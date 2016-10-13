using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ContentParser.Tests
{
    [TestClass]
    public class ParserTests
    {
        private string styleString;
        private string dataString;

        [TestInitialize]
        public void Initialize()
        {
           
        }

        [TestMethod]
        public void TestStyleMapping()
        {
            Generate5eStyle();
            Generate5eData();

            var result = ContentParser.ConvertToHtml(dataString, styleString);

            Assert.IsNotNull(result);

        }


        private void Generate5eStyle()
        {
            dynamic style = new ExpandoObject();

            style.sectionorder = new[] { "header", "divider", "general", "divider", "statblock", "divider", "abilities", "divider", "tactics", "actiondivider", "actions" };

            style.sections = new Dictionary<string, ExpandoObject>();
                  
            style.sections["divider"] = new ExpandoObject();
            style.sections["divider"].type = "divider";
                  
            style.sections["header"] = new ExpandoObject();
            style.sections["header"].type = "items";
            style.sections["header"].itemorder = new[] { "name", "type" };
            style.sections["header"].items = new Dictionary<string, string>();
            style.sections["header"].items["name"] = "## [name]";
            style.sections["header"].items["type"] = "*[type]*";
                  
            style.sections["general"] = new ExpandoObject();
            style.sections["general"].type = "items";
            style.sections["general"].itemorder = new[] {"ac","hp","spd"};
            style.sections["general"].items = new Dictionary<string, string>();
            style.sections["general"].items["ac"] = "**Armor Class:** [ac]";
            style.sections["general"].items["hp"] = "**Hit Points:** [hp] ([hd])";
            style.sections["general"].items["spd"] = "**Speed:** [spd]";
                  
            style.sections["statblock"] = new ExpandoObject();
            style.sections["statblock"].type = "table";
            style.sections["statblock"].headers = new[] { "STR", "DEX", "CON", "INT", "WIS", "CHA" };
            style.sections["statblock"].row = "|[str]|[dex]|[con]|[intel]|[wis]|[cha]|";
                  
            style.sections["abilities"] = new ExpandoObject();
            style.sections["abilities"].type = "list";
            style.sections["abilities"].format = "**[key]:** [value]";
                 
            style.sections["tactics"] = new ExpandoObject();
            style.sections["tactics"].type = "list";
            style.sections["tactics"].format = "**[key]:** [value]";
                 
            style.sections["actiondivider"] = new ExpandoObject();
            style.sections["actiondivider"].type = "header";
            style.sections["actiondivider"].itemorder = new [] {"title", "divider"};
            style.sections["actiondivider"].items = new Dictionary<string, string>();
            style.sections["actiondivider"].items["title"] = "### Actions";
            style.sections["actiondivider"].items["divider"] = "___";
                 
            style.sections["actions"] = new ExpandoObject();
            style.sections["actions"].type = "list";
            style.sections["actions"].format = "**[key]:** [value]";

            styleString = JsonConvert.SerializeObject(style);
        }

        private void Generate5eData()
        {
            dynamic data = new ExpandoObject();

            data.name = "Ghost Pigeon";
            data.type = "Small fiend, chaotic sloppy";
            data.ac = "18";
            data.hp = "25";
            data.hd = "3d8+2";
            data.spd = "30 ft.";
            data.str = "12";
            data.dex = "13";
            data.con = "12";
            data.intel = "13";
            data.wis = "12";
            data.cha = "13";
            data.abilities = new Dictionary<string, string>
            {
                {"Condition Immunities", "melancholy, groggy"},
                {"Senses","passive Perception"},
                {"Languages","Common"},
                {"Challenge","12 (5173 XP)"}
            };
            data.tactics = new Dictionary<string, string>
            {
                {"Pack Tactics","These guys work together. Like super well, you don't even know." }
            };
            data.actions = new Dictionary<string, string>
            {
                {"Crossface Suplex.","*Melee Weapon Attack:* +4 to hit, reach 5ft., one target. *Hit* 5 (1d6 + 2) "},
                {"Jumping Driver.","*Melee Weapon Attack:* +4 to hit, reach 5ft., one target. *Hit* 5 (1d6 + 2) "},
            };

            dataString = JsonConvert.SerializeObject(data);
        }
    }
}
