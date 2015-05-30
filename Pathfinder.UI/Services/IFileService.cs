using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Pathfinder.Core;

namespace Pathfinder.UI.Services
{
    public interface IFileService
    {
        World<bool> LoadWorld(string path);

        void SaveWorld(World<bool> world, string path);
    }

    public class DefaultFileService : IFileService
    {
        public World<bool> LoadWorld(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open))
            using (var reader = XmlReader.Create(fileStream))
            {
                return this.LoadWorld(reader);
            }
        }

        public void SaveWorld(World<bool> world, string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Create))
            using( var writer = XmlWriter.Create(fileStream))
            {
                SaveWorld(writer, world);
            }
        }


        private World<bool> LoadWorld(System.Xml.XmlReader reader)
        {
            // Write Map Root
            reader.ReadToDescendant("map");

            if (reader.Name != "map")
                throw new ApplicationException("no map found.");

            // Load Dimensions
            var width = int.Parse(reader.GetAttribute("width"));
            var height = int.Parse(reader.GetAttribute("height"));

            var world = new World<bool>(width, height, true);

            reader.Read();
            while (reader.Name == "cell")
            {
                var x = int.Parse(reader.GetAttribute("x"));
                var y = int.Parse(reader.GetAttribute("y"));
                var v = bool.Parse(reader.GetAttribute("v"));

                world[x, y] = v;

                reader.Read();
            }

            reader.Read();

            return world;
        }

        private void SaveWorld(System.Xml.XmlWriter writer, World<bool> world)
        {
            // Write Map Root
            writer.WriteStartElement("map");

            // Write Size Dimensions
            writer.WriteAttributeString("width", world.Width.ToString());
            writer.WriteAttributeString("height", world.Height.ToString());

            // Write cell states
            for (int x = 0; x < world.Width; x++)
            {
                for (int y = 0; y < world.Height; y++)
                {
                    writer.WriteStartElement("cell");

                    writer.WriteAttributeString("x", x.ToString());
                    writer.WriteAttributeString("y", y.ToString());
                    writer.WriteAttributeString("v", world[x, y].ToString());

                    writer.WriteEndElement();
                }
            }

            //End Map Root
            writer.WriteEndElement();
        }
    }
}
