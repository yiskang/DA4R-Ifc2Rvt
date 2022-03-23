// (C) Copyright 2022 by Autodesk, Inc. 
//
// Permission to use, copy, modify, and distribute this software
// in object code form for any purpose and without fee is hereby
// granted, provided that the above copyright notice appears in
// all copies and that both that copyright notice and the limited
// warranty and restricted rights notice below appear in all
// supporting documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK,
// INC. DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL
// BE UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is
// subject to restrictions set forth in FAR 52.227-19 (Commercial
// Computer Software - Restricted Rights) and DFAR 252.227-7013(c)
// (1)(ii)(Rights in Technical Data and Computer Software), as
// applicable.
//

using System;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using DesignAutomationFramework;

namespace Autodesk.ADN.Ifc2Rvt
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class MainApp : IExternalDBApplication
    {
        public ExternalDBApplicationResult OnStartup(ControlledApplication application)
        {
            DesignAutomationBridge.DesignAutomationReadyEvent += HandleDesignAutomationReadyEvent;
            return ExternalDBApplicationResult.Succeeded;
        }

        public ExternalDBApplicationResult OnShutdown(ControlledApplication application)
        {
            return ExternalDBApplicationResult.Succeeded;
        }

        private void HandleDesignAutomationReadyEvent(object sender, DesignAutomationReadyEventArgs e)
        {
            LogTrace("Design Automation Ready event triggered...");
            e.Succeeded = true;
            e.Succeeded = this.DoTask(e.DesignAutomationData);
        }
        private bool DoTask(DesignAutomationData data)
        {
            if (data == null)
                return false;

            Application app = data.RevitApp;
            if (app == null)
            {
                LogTrace("Error occured");
                LogTrace("Invalid Revit App");
                return false;
            }

            //string modelPath = data.FilePath;
            //if (string.IsNullOrWhiteSpace(modelPath))
            //{
            //    LogTrace("Error occured");
            //    LogTrace("Invalid File Path");
            //    return false;
            //}

            //var doc = data.RevitDoc;
            //if (doc == null)
            //{
            //    LogTrace("Error occured");
            //    LogTrace("Invalid Revit DB Document");
            //    return false;
            //}

            LogTrace("Opening IFC file `input.ifc`...");
            try
            {
                var ifcImportOpts = new Autodesk.Revit.DB.IFC.IFCImportOptions();
                ifcImportOpts.AutoJoin = true;
                ifcImportOpts.AutocorrectOffAxisLines = true;

                var ifcDoc = app.OpenIFCDocument("input.ifc", ifcImportOpts);
                ifcDoc.SaveAs("output.ifc.rvt");
                ifcDoc.Close(false);
            }
            catch (Autodesk.Revit.Exceptions.InvalidPathArgumentException ex)
            {
                this.PrintError(ex);
                return false;
            }
            catch (Autodesk.Revit.Exceptions.ArgumentException ex)
            {
                this.PrintError(ex);
                return false;
            }
            catch (Autodesk.Revit.Exceptions.InvalidOperationException ex)
            {
                this.PrintError(ex);
                return false;
            }
            catch (Exception ex)
            {
                this.PrintError(ex);
                return false;
            }

            LogTrace("Successfully resaved `input.ifc` to `output.ifc.rvt`...");

            return true;
        }

        private void PrintError(Exception ex)
        {
            LogTrace("Error occured");
            LogTrace(ex.Message);

            if (ex.InnerException != null)
                LogTrace(ex.InnerException.Message);
        }

        /// <summary>
        /// This will appear on the Design Automation output
        /// </summary>
        private static void LogTrace(string format, params object[] args)
        {
#if DEBUG
            System.Diagnostics.Trace.WriteLine(string.Format(format, args));
#endif
            System.Console.WriteLine(format, args);
        }
    }
}
