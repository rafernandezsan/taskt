﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Data;
using System.Text;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command gets text from a specified Excel Range and put it into a DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a value from a specific range.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    public class ExcelGetRangeCommandAsDT : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the First Cell Location (ex. A1 or B2)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual location of the cell.")]
        [Attributes.PropertyAttributes.SampleUsage("A1, B10, [vAddress]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ExcelCellAddress1 { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select output Option")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Datatable")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Delimited String")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate whether this command should return a datatable or Delimited String")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Datatable** or **Delimited String**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Output { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Second Cell Location (ex. A1 or B2)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual location of the cell.")]
        [Attributes.PropertyAttributes.SampleUsage("A1, B10, [vAddress]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ExcelCellAddress2 { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Assign to Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_userVariableName { get; set; }

        public ExcelGetRangeCommandAsDT()
        {
            this.CommandName = "ExcelGetRangeCommandAsDT";
            this.SelectionName = "Get Range As Datatable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {

           
                var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);
            var targetAddress1 = v_ExcelCellAddress1.ConvertToUserVariable(sender);
            var targetAddress2 = v_ExcelCellAddress2.ConvertToUserVariable(sender);

            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
            var cellValue = excelSheet.Range[targetAddress1, targetAddress2];
            if (v_Output == "Datatable")
            {

                List<object> lst = new List<object>();
                int rw = cellValue.Rows.Count;
                int cl = cellValue.Columns.Count;
                int rCnt;
                int cCnt;
                string str;
                DataTable DT = new DataTable();
                for (rCnt = 1; rCnt <= rw; rCnt++)
                {

                    DataRow newRow = DT.NewRow();
                    for (cCnt = 1; cCnt <= cl; cCnt++)
                    {

                        if (((cellValue.Cells[rCnt, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2) != null)
                        {
                            if (!DT.Columns.Contains(cCnt.ToString()))
                            {
                                DT.Columns.Add(cCnt.ToString());
                            }
                            newRow[cCnt.ToString()] = ((cellValue.Cells[rCnt, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2).ToString();


                        }
                    }
                    DT.Rows.Add(newRow);
                }
                Script.ScriptVariable newDataset = new Script.ScriptVariable
                {
                    VariableName = v_userVariableName,
                    VariableValue = DT
                };

                engine.VariableList.Add(newDataset);
            }
            if(v_Output == "Delimited String")
            {
                List<object> lst = new List<object>();
                int rw = cellValue.Rows.Count;
                int cl = cellValue.Columns.Count;
                int rCnt;
                int cCnt;
                string str;

                //Get Range from Excel sheet and add to list of strings.
                for (rCnt = 1; rCnt <= rw; rCnt++)
                {

                    for (cCnt = 1; cCnt <= cl; cCnt++)
                    {
                        if (((cellValue.Cells[rCnt, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2) != null)
                        {
                            str = ((cellValue.Cells[rCnt, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2).ToString();
                            lst.Add(str);
                        }

                    }

                }
                string output = String.Join(",", lst);
                v_ExcelCellAddress1 = output;

                //Store Strings of comma seperated values into user variable
                output.StoreInUserVariable(sender, v_userVariableName);
            }

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellAddress1", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellAddress2", this, editor));
            //create control for variable name
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_Output", this, editor));


            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Get Value Between '" + v_ExcelCellAddress1 + "And " + v_ExcelCellAddress2 + "' and apply to variable '" + v_userVariableName + "'from, Instance Name: '" + v_InstanceName + "']";
        }
    }
}