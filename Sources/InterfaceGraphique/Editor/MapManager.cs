﻿using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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

        // Add NumberOfPlayers?
    }

    public class MapManager
    {
        private MapService mapService;
        // The meta data of the current edited map have to be ALWAYS accurate:
        private MapMetaData currentMapInfo;

        public MapManager(MapService mapService)
        {
            this.mapService = mapService;
            this.currentMapInfo = new MapMetaData();
        }

        public void resetMapInfo()
        {
            this.currentMapInfo = new MapMetaData
            {
                Creator = Program.user?.Username
            };
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
                this.currentMapInfo = new MapMetaData
                {
                    savedOnce = true,
                    Creator = Program.user?.Username,
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

            // We have to update the properties of the current map:
            this.currentMapInfo.savedOnce = true;
            this.currentMapInfo.savedOnline = false;
        }

        private async Task SaveOnlineMap()
        {
            // First, we fetch the JSON of the map:
            StringBuilder sb = new StringBuilder(2000);
            FonctionsNatives.getMapJson(Program.GeneralProperties.GetCoefficientValues(), sb);
            string json = sb.ToString();

            MapEntity map = new MapEntity
            {
                Id = this.currentMapInfo.Id,
                Creator = this.currentMapInfo.Creator,
                MapName = this.currentMapInfo.Name,
                LastBackup = DateTime.Now,
                Json = json,
                Private = this.currentMapInfo.Private,
                Password = this.currentMapInfo.Password
            };

            bool saved = await this.mapService.SaveMap(map);

            // As Linq doesn't manage db-generated entries with MySQL, we have to
            // manually query the id of a newly-saved map:
            if (!this.currentMapInfo.savedOnce)
            {
                int? id = await this.mapService.GetMapID(map);

                Debug.Assert(id != null);
                this.currentMapInfo.Id = id;
            }

            if (!saved)
            {
                MessageBox.Show(
                    @"Impossible de sauvegarder la carte. Veuillez ré-essayer plus tard.",
                    @"Internal error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
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
                    Creator = Program.user.Username,
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