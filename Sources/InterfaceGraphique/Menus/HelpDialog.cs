using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceGraphique {

    ///////////////////////////////////////////////////////////////////////////
    /// @class EditorHelp
    /// @brief UI de la fentre d'information (aide)
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class EditorHelp : Form {

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe EditorHelp
        ///
        ////////////////////////////////////////////////////////////////////////
        public EditorHelp() {
            InitializeComponent();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Récupère le fichier texte d'aide pour l'éditeur et assigne son
        /// contenu au label dans le form.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public void ShowEditorHelpText() {
            this.Text = "Aide - Mode éditeur";

            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"media/texts/InformationsEditeur.txt"));
            string message = File.ReadAllText(path);

            this.HelpText.Text = message;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Récupère le fichier texte d'aide pour le mode test et assigne son
        /// contenu au label dans le form.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public void ShowTestHelpText() {
            this.Text = "Aide - Mode test";

            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"media/texts/InformationsTestMode.txt"));
            string message = File.ReadAllText(path);

            this.HelpText.Text = message;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Récupère le fichier texte d'aide pour partie rapide et assigne son
        /// contenu au label dans le form.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public void ShowQuickPlayHelpText() {
            this.Text = "Aide - Mode partie rapide";

            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"media/texts/InformationsQuickPlay.txt"));
            string message = File.ReadAllText(path);

            this.HelpText.Text = message;
        }
    }
}
