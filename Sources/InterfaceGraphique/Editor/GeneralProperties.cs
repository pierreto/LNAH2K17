using Newtonsoft.Json;
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
    /// @class GeneralProperties
    /// @brief UI de la fenetre de propriété de la carte
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class GeneralProperties : Form {

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe GeneralProperties
        ///
        ////////////////////////////////////////////////////////////////////////
        public GeneralProperties() {
            InitializeComponent();
            InitializeEvents();
            LoadGeneralProperties();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Initialise les events sur la form courrante
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeEvents() {
            this.Button_SaveChanges.Click += (sender, e) => { SaveGeneralProperties(sender, e); this.Close(); };
            this.Button_Cancel.Click += (sender, e) => this.Close();
            this.FormClosing += (sender, e) => LoadGeneralProperties();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Permet la sauvegarde des inputs de la form losque le bouton 
        /// sauvegarder est utilisé.
        /// 
        /// @param[in]  sender : Objet qui a causé l'évènement
        /// @param[in]  e      : Arguments de l'évènement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void SaveGeneralProperties(object sender, EventArgs e) {
            coefficientFriction = (float)this.Input_CoefFriction.Value;
            coefficientRebond = (float)this.Input_CoefRebound.Value;
            coefficientAcceleration = (float)this.Input_CoefAcceleration.Value;

            if (Program.FormManager.CurrentForm.GetType() == typeof(Editeur))
            {
                Program.Editeur.HandleCoefficientChanges(coefficientFriction, coefficientAcceleration, coefficientRebond);
            }

            LoadGeneralProperties();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Permet le chargement des propriétés à afficher dans les 
        /// inputBox de la form.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void LoadGeneralProperties() {
            this.Input_CoefFriction.Text = coefficientFriction.ToString();
            this.Input_CoefRebound.Text = coefficientRebond.ToString();
            this.Input_CoefAcceleration.Text = coefficientAcceleration.ToString();

            FonctionsNatives.setCoefficients(coefficientFriction, coefficientAcceleration, coefficientRebond);
     
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Réinitialise les propriétés globales de la table.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public void ResetProperties() {
            coefficientFriction = DefaultValues.coefficientFriction;
            coefficientRebond = DefaultValues.coefficientRebond;
            coefficientAcceleration = DefaultValues.coefficientAcceleration;

            LoadGeneralProperties(); 
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Retourne les valeurs des trois coefficients de la carte en cours
        ///
        /// @return Les valeurs de coefficient en tableau.
        ///
        ////////////////////////////////////////////////////////////////////////
        public float[] GetCoefficientValues() {
            return new float[] { coefficientFriction, coefficientRebond, coefficientAcceleration };
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Charge les valeurs des trois coefficients de la carte en cours.
        /// 
        /// @param[in]  coefficients : Les valeurs de coefficient en tableau.
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public void SetCoefficientValues(float[] coefficients) {
            coefficientFriction = coefficients[0];
            coefficientRebond = coefficients[1];
            coefficientAcceleration = coefficients[2];

            LoadGeneralProperties();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private float coefficientFriction = DefaultValues.coefficientFriction;
        private float coefficientRebond = DefaultValues.coefficientRebond;
        private float coefficientAcceleration = DefaultValues.coefficientAcceleration;


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Classe contenant les valeurs par défaut utilisées pour la classe
        /// GeneralProperties
        ///
        ////////////////////////////////////////////////////////////////////////
        private class DefaultValues {
            public static float coefficientFriction = 1.0f;
            public static float coefficientRebond = 0;
            public static float coefficientAcceleration = 40.0f;
        }
    }
}
