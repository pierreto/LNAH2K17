using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceGraphique.Editor
{
    public class MapMetaData
    {
        // Indicates if our current map has been saved at least once and online or not:
        public bool savedOnce;
        public bool savedOnline;

        public int? Id;
        public string Creator;
        public string Name;
        public bool Private;
        public string Password;

        public DateTime LastBackup;
    }

    public class MapManager
    {
        private MapService mapService;
        // The meta data of the current edited map have to be ALWAYS accurate:
        private MapMetaData currentMapInfo;
        private Object saveMapLock = new object();
        private Object saveIconLock = new object();

        public MapManager(MapService mapService)
        {
            this.mapService = mapService;
            this.currentMapInfo = new MapMetaData();
        }

        public void resetMapInfo()
        {
            string creator = User.Instance.UserEntity?.Username;
            this.currentMapInfo = new MapMetaData{ Creator = creator };
        }

        private void LoadJSON(string json)
        {
            float[] coefficients = new float[3];
            FonctionsNatives.chargerCarte(new StringBuilder(json), coefficients);
            Program.GeneralProperties.SetCoefficientValues(coefficients);
        }

        public void OpenLocalMap() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JSON Files (JSON)|*.json";
            ofd.InitialDirectory = Directory.GetCurrentDirectory() + "\\zones";

            if (ofd.ShowDialog() == DialogResult.OK) {
                this.LoadJSON(File.ReadAllText(ofd.FileName));

                // We have to set the properties of the new current map:
                string creator = User.Instance.UserEntity?.Username;
                this.currentMapInfo = new MapMetaData
                {
                    savedOnce = true,
                    Creator = creator,
                    Name = ofd.FileName
                };
            }
        }

        // TODO:
        // Currently receiving a MapEntity from the wpf view model, where the json is null
        // we have to fetch it using the map id. Receiving a MapMetaData directly
        // should be cleaner.
        public async Task OpenOnlineMap(MapEntity mapMetaData)
        {
            Debug.Assert(mapMetaData.Json == null & mapMetaData.Id != null);

            if (mapMetaData.Private)
            {
                InterfaceGraphique.Editor.OpenPrivateMapForm passwordForm = new InterfaceGraphique.Editor.OpenPrivateMapForm();

                if (passwordForm.ShowDialog() != DialogResult.OK || passwordForm.Text_MapPassword.Text != mapMetaData.Password)
                {
                    MessageBox.Show(
                        @"Mot de passe erroné.",
                        @"Erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            MapEntity realMap = await this.mapService.GetMap((int) mapMetaData.Id);
            this.LoadJSON(realMap.Json);

            // We have to set the properties of the new current map:
            this.currentMapInfo = new MapMetaData
            {
                savedOnce = true,
                savedOnline = true,
                Id = mapMetaData.Id,
                Creator = mapMetaData.Creator,
                Name = mapMetaData.MapName,
                Private = mapMetaData.Private,
                Password = mapMetaData.Password
            };
        }

        private void SaveLocalMap()
        {
            StringBuilder filePath = new StringBuilder(this.currentMapInfo.Name);
            FonctionsNatives.enregistrerSous(filePath, Program.GeneralProperties.GetCoefficientValues());
            byte[] bytes = new byte[128*128*3];
            FonctionsNatives.getMapIcon(bytes);

            // We have to update the properties of the current map:
            this.currentMapInfo.savedOnce = true;
            this.currentMapInfo.savedOnline = false;
        }

        public void SaveIcon()
        {
            byte[] icon = new byte[500*500*3];
            FonctionsNatives.getMapIcon(icon);
            var str = "";
            Bitmap bmp = new Bitmap(500, 500, PixelFormat.Format24bppRgb);
            BitmapData bmpData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(icon, 0, bmpData.Scan0, icon.Length);
            bmp.UnlockBits(bmpData);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Jpeg);
            byte[] res = ms.ToArray();
            str = Convert.ToBase64String(res);
            MapEntity map = new MapEntity
            {
                Id = this.currentMapInfo.Id,
                Icon = str
            };
            Task.Run(async () => await this.mapService.SaveMap(map));
        }

        private async Task SaveOnlineMap()
        {
            // First, we fetch the JSON of the map:
            ThreadLocal<string> json = new ThreadLocal<string>();
            lock (this.saveMapLock)
            {
                try
                {
                    StringBuilder sb = new StringBuilder(60000);
                    FonctionsNatives.getMapJson(Program.GeneralProperties.GetCoefficientValues(), sb);
                    json.Value = sb.ToString();
                }
                catch
                {
                    MessageBox.Show(
                         @"Impossible de sauvegarder la carte : celle-ci contient trop d'objets. :-(",
                         @"Internal error",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            ThreadLocal<MapEntity> map = new ThreadLocal<MapEntity>(
                () =>
                {
                    return new MapEntity
                    {
                        Id = this.currentMapInfo.Id,
                        Creator = this.currentMapInfo.Creator,
                        MapName = this.currentMapInfo.Name,
                        CreationDate = DateTime.Now,
                        Json = json.Value,
                        Private = this.currentMapInfo.Private,
                        Password = this.currentMapInfo.Password,
                        LastBackup = DateTime.Now
                    };
                });

            ThreadLocal<bool> saved = new ThreadLocal<bool>();

            if (this.currentMapInfo.savedOnce)
            {
                saved.Value = await this.mapService.SaveMap(map.Value);
            }
            else
            {
                int? savedMapId = await this.mapService.SaveNewMap(map.Value);
                if (savedMapId != null)
                {
                    map.Value.Id = savedMapId; // needed to join the online edition mode
                    this.currentMapInfo.Id = savedMapId;
                    saved.Value = true;
                }
            }

            if (!saved.Value)
            {
                MessageBox.Show(
                    @"Impossible de sauvegarder la carte. Veuillez ré-essayer plus tard.",
                    @"Internal error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // we just saved the map for the first time so
                // we have to join the online edition mode:
                if (!this.currentMapInfo.savedOnce)
                {
                    Program.Editeur.CurrentState = Program.Editeur.onlineState;
                    Program.Editeur.CurrentState.JoinEdition(map.Value);
                }

                // we have to update the properties of the current map:
                this.currentMapInfo.savedOnce = true;
                this.currentMapInfo.savedOnline = true;
            }
        }

        public void ManageSavingLocalMap()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "json";
            sfd.AddExtension = true;
            sfd.Filter = "JSON Files (JSON)|*.json";
            sfd.InitialDirectory = Directory.GetCurrentDirectory() + "\\zones";

            if (sfd.ShowDialog() == DialogResult.OK) {
                this.currentMapInfo.Name = sfd.FileName;
                SaveLocalMap();
            }
        }

        // Save the map on the server, as a new fresh map
        // -> could be a fork of an existing online map so we have
        // to update the meta data to change the ID and the Creator!
        public async Task ManageSavingOnlineMap()
        {
            Editor.SaveMapOnlineForm form = new Editor.SaveMapOnlineForm();

            if (form.ShowDialog() == DialogResult.OK && form.Text_MapName.Text.Length > 0)
            {
                this.currentMapInfo = new MapMetaData
                {
                    Creator = User.Instance.UserEntity.Username,
                    Name = form.Text_MapName.Text
                };

                if (form.Button_PrivateMap.Checked)
                {
                    if (form.Text_PwdMap.Text.Length >= 5)
                    {
                        this.currentMapInfo.Private = true;
                        this.currentMapInfo.Password = form.Text_PwdMap.Text;

                        await SaveOnlineMap();
                    }
                    else
                    {
                        MessageBox.Show(
                            @"Le mot de passe pour accéder à la carte doit contenir au moins 5 caractères.",
                            @"Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    await SaveOnlineMap();
                }
            }
            else
            {
                MessageBox.Show(
                    @"Le nom de carte ne peut être vide.",
                    @"Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool CurrentMapAlreadySaved()
        {
            return this.currentMapInfo.savedOnce;
        }

        public async Task SaveMap() {
            if (this.currentMapInfo.savedOnce)
            {
                if (this.currentMapInfo.savedOnline)
                {
                    await SaveOnlineMap();
                }
                else
                {
                    Debug.Assert(File.Exists(this.currentMapInfo.Name));
                    SaveLocalMap();
                }
            }
            else
            {
                Editor.SaveMapForm form = new Editor.SaveMapForm();
                form.ShowDialog();
                if (form.SaveOnline)
                {
                    await ManageSavingOnlineMap();
                }
                else
                {
                    ManageSavingLocalMap();
                }
            }
        }
    }
}