using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLC_Controller {
    //TODO: Declare global variables and settings here as static
    internal static partial class Global {
        //Setting files
        // Directory structure:
        // Application executable directory\
        //   |-> ProductName\
        //   |    |-> setting.json   //general settings
        //   |    |-> daq.json       //daq settings
        //   |    |-> plc.json       //plc settings
        //   |    |-> robot.json     //robot settings
        //   |    |-> scanner.json   //scanner settings
        //   |    |-> camera.json    //camera settings
        //   |    |-> Language\      //language directory (automatic)
        //   |    |    |-> ko\      
        //   |    |    |     |-> Profiles\      
        //   |    |    |     |     |-> User defined profiles\      


        //We are using directly the application directory to store local settings
        ////Software back-up is then simply performed by zipping the entired app folder
        ////public static string SettingDir = $"./{Application.ProductName}.Settings";
        //public static string GeneralSettingFile => SettingDir + "/setting.json";
        //public static string PickUFImageFile => SettingDir + "/pick_uf.png";
        //public static string InstallUFImageFile => SettingDir + "/install_uf.png";
        //public static string ResultDir => Global.Setting.ResultDir;
        //public static string LogDir => ResultDir + "/Logs";

        //public static string ResetTime = "03:00";
        //public static Color ColorOK = NiceColor.Emerald;
        //public static Color ColorNG = NiceColor.Alizarin;
        //public static Color ColorRESET = NiceColor.SunFlower;
        //public static Color ColorNumberRESET = SolarizedColor.Base03;
        //public static Color ColorConnected = NiceColor.Emerald;
        //public static Color ColorDisconnected = NiceColor.Alizarin;

        //ENVIRONMENT VARS
        ////General settings
        //public static FormMain MainForm = null;

        public const int ID_LH = 0;
        public const int ID_RH = 1;
        public const int ID_MID = 2;

        //====================================================================================================
        #region MODEL
        //Models
        //public static string ModelSettingFile => SettingDir + "/model.json";
        //public static List<ModelConfig> Models = new List<ModelConfig>();

        //Extra model initialization
        //static void InitModels()
        //{
        //    try
        //    {
        //        Models?.ForEach(x => x.Init());
        //    }
        //    catch (Exception ex) { LotusAPI.Logger.Error(ex.Message); }
        //}

        //public static void LoadModels()
        //{
        //    try
        //    {
        //        Logger.Info("Loading models...");
        //        Models = JsonUtils.Read<List<ModelConfig>>(Json.ReadFromFile(ModelSettingFile)) ?? new List<ModelConfig>();
        //        InitModels();
        //    }
        //    catch (Exception ex) { LotusAPI.Logger.Error(ex.Message); }
        //}

        //public static void SaveModels()
        //{
        //    try
        //    {
        //        if (DialogUtils.AskForPermission())
        //        {
        //            Logger.Info($"Saving model to {ModelSettingFile}...");
        //            JsonUtils.ToJson(Models).Save(ModelSettingFile);
        //            DialogUtils.ShowInfoMsg($"Model saved! ({ModelSettingFile})");
        //            InitModels();
        //        }
        //    }
        //    catch (Exception ex) { LotusAPI.Logger.Error(ex.Message); }
        //}
        #endregion

        //====================================================================================================
        #region DATABASE
        //Database
        //public static string DbFile => SettingDir + "/database.db";
        //public static DB DB = new DB();
        //public static void InitDB()
        //{
        //    try
        //    {
        //        DB.Connect();
        //    }
        //    catch (Exception ex) { LotusAPI.Logger.Error(ex.Message); }
        //}
        #endregion


        //================================================================================
        #region LOCALIZATION
        //public static MessageText MsgTxt = new MessageText();
        //public static ErrorText ErrTxt = new ErrorText();
        //public static DBText DbTxt = new DBText();
        //public static string LanguageDir => SettingDir + "/Language";
        //public static string CurrentLanguageDir => LanguageDir + "/" + Setting.Language;
        //public static void Localize()
        //{
        //    try
        //    {
        //        //SETTING
        //        TypeUtils.LoadTypeInfo(CurrentLanguageDir);

        //        Setting.Localize();
        //#if USE_PLC
        //                Plc.Setting.Localize();
        //#endif 

        //#if USE_ROBOT
        //                for(int i = 0; i < Global.ROBOT_COUNT; i++) {
        //                    Robots[i].Setting.Localize();
        //                }
        //#endif
        //                TypeUtils.SetPropertyInfo_<ModelConfig>();
        //                TypeUtils.SetPropertyInfo_<PinInfo>();
        //                TypeUtils.SetPropertyInfo_<IPLC.MemoryBlock>();

        //                MsgTxt = TypeUtils.GetStringResource_<MessageText>();
        //                ErrTxt = TypeUtils.GetStringResource_<ErrorText>();
        //                DbTxt = TypeUtils.GetStringResource_<DBText>();

        //                TypeUtils.SaveTypeInfo(CurrentLanguageDir);
        //            }
        //            catch (Exception ex) { LotusAPI.Logger.Error(ex.Message); }
        //        }
        #endregion


        public static void InitGlobalSetting() {
            //First we need to load previous setting
        }
        //Init app
        //        public static void Init()
        //        {
        //            InitDB();

        //#if USE_PLC
        //            InitPlc();
        //#endif

        //#if USE_CAMERA
        //            InitCameras();
        //            InitLights();
        //#endif

        //#if USE_ROBOT
        //            InitRobots();
        //#endif

        //#if USE_SCANNER
        //            InitScanners();
        //#endif

        //#if USE_SCANNER && USE_ROBOT
        //            InitHandEyes();
        //#endif

        //            //LoadModels();
        //            //Localize();
        //        }

        //Terminate hardware
        public static void Terminate() {
#if USE_CAMERA
            TerminateCameras();
            TerminateLights();
#endif

#if USE_SCANNER
            TerminateScanners();
#endif

#if USE_ROBOT
            TerminateRobots();
#endif

#if USE_PLC
            TerminatePlc();
#endif

            //Library.Terminate();
        }
    }
}
