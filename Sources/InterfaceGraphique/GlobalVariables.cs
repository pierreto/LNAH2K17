using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceGraphique {

    ///////////////////////////////////////////////////////////////////////////
    /// @class GlobalVariables
    /// @brief Classe statique contenant les variables globales
    /// @author Julien Charbonneau
    ///////////////////////////////////////////////////////////////////////////
    static class GlobalVariables {
        public const double deplacementVue = 0.1;
        public const float speedMaillet = 2.0f;
        public static Color defaultGrey = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
    }


    ///////////////////////////////////////////////////////////////////////////
    /// 
    /// Classe décrivant la stucture d'un profil de joueur virtuel
    ///
    ///////////////////////////////////////////////////////////////////////////
    public class PlayerProfile {
        public PlayerProfile(string name, int speed, int passivity) {
            Name = name;
            Speed = speed;
            Passivity = passivity;
        }

        public string Name;
        public int Speed;
        public int Passivity;
    }


    ///////////////////////////////////////////////////////////////////////////
    /// 
    /// Classe qui redéfini le rendering du ToolStripProfessionalRenderer
    /// selon les besoin du UI
    /// 
    ///////////////////////////////////////////////////////////////////////////
    class Renderer_MenuBar : ToolStripProfessionalRenderer {
        public Renderer_MenuBar() : base(new Colors_MenuBar()) { }
    }


    ///////////////////////////////////////////////////////////////////////////
    /// 
    /// Classe qui redéfini le rendering du ProfessionalColorTable
    /// selon les besoin du UI
    ///
    ///////////////////////////////////////////////////////////////////////////
    class Colors_MenuBar : ProfessionalColorTable {
        public override Color MenuItemSelected {
            get { return SystemColors.ControlDarkDark; }
        }
        public override Color MenuItemSelectedGradientBegin {
            get { return SystemColors.ControlDarkDark; }
        }
        public override Color MenuItemSelectedGradientEnd {
            get { return SystemColors.ControlDarkDark; }
        }
        public override Color MenuItemPressedGradientBegin {
            get { return SystemColors.ControlDarkDark; }
        }
        public override Color MenuItemPressedGradientMiddle {
            get { return SystemColors.ControlDarkDark; }
        }
        public override Color MenuItemPressedGradientEnd {
            get { return SystemColors.ControlDarkDark; }
        }
    }


    ///////////////////////////////////////////////////////////////////////////
    /// 
    /// Enum contenant les différents état possible du modele etat du programme
    ///
    ///////////////////////////////////////////////////////////////////////////
    enum MODELE_ETAT {
        AUCUN = 0,
        SELECTION = 1,
        CREATION_ACCELERATEUR = 2,
        CREATION_MURET = 3,
        CREATION_PORTAIL = 4,
        DEPLACEMENT = 5,
        ROTATION = 6,
        MISE_A_ECHELLE = 7,
        DUPLIQUER = 8,
        ZOOM = 9,
        POINTS_CONTROLE = 10,
        JEU = 11,
        JEU_ONLINE=12
    };
}
