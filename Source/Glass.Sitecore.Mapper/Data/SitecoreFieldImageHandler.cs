/*
   Copyright 2011 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Glass.Sitecore.Mapper.FieldTypes;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Configuration;
using Sitecore.Data;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Glass.Sitecore.Mapper.Data
{
    public class SitecoreFieldImageHandler : AbstractSitecoreField
    {

        public override object GetValue(global::Sitecore.Data.Items.Item item,  ISitecoreService service)
        {
            var itemField = base.GetField(item);

            if (itemField == null) return null;

            Image img = new Image();
            ImageField scImg = new ImageField(itemField);

            int height = 0;
            int.TryParse(scImg.Height, out height);
            int width = 0;
            int.TryParse(scImg.Width, out width);
            int hSpace = 0;
            int.TryParse(scImg.HSpace, out hSpace);
            int vSpace = 0;
            int.TryParse(scImg.VSpace, out vSpace);

            img.Alt = scImg.Alt;
            img.Border = scImg.Border;
            img.Class = scImg.Class;
            img.Height = height;
            img.HSpace = hSpace;
            img.MediaId = scImg.MediaID.Guid;
            if(scImg.MediaItem != null)
                img.Src = HttpUtility.UrlPathEncode(MediaManager.GetMediaUrl(scImg.MediaItem));
            img.VSpace = vSpace;
            img.Width = width;

            return img;
        }
        public override void SetValue( global::Sitecore.Data.Items.Item item, object value,  ISitecoreService service)
        {

            Image img = value as Image;

            Field itemField = base.GetField(item);

            if (itemField == null) return;

            ImageField scImg = new ImageField(itemField);

            if (img == null)
            {
                scImg.Clear();
                return;
            }

            if (scImg.MediaID.Guid != img.MediaId)
            {
                //this only handles empty guids, but do we need to remove the link before adding a new one?
                if (img.MediaId == Guid.Empty)
                {
                    ItemLink link = new ItemLink(item.Database.Name, item.ID, scImg.InnerField.ID, scImg.MediaItem.Database.Name, scImg.MediaID, MediaManager.GetMediaUrl(scImg.MediaItem));
                    scImg.RemoveLink(link);
                }
                else
                {
                    ID newId = new ID(img.MediaId);
                    Item target = item.Database.GetItem(newId);
                    if (target != null)
                    {
                        scImg.MediaID = newId;
                        ItemLink link = new ItemLink(item.Database.Name, item.ID, scImg.InnerField.ID, target.Database.Name, target.ID, target.Paths.FullPath);
                        scImg.UpdateLink(link);
                    }
                    else throw new MapperException("No item with ID {0}. Can not update Media Item field".Formatted(newId));
                }
            }

            scImg.Height = img.Height.ToString();
            scImg.Width = img.Width.ToString();
            scImg.HSpace = img.HSpace.ToString();
            scImg.VSpace = img.VSpace.ToString();
            scImg.Alt = img.Alt;
            scImg.Border = img.Border;
            scImg.Class = img.Class;
          
           

        }

        public override object GetFieldValue(string fieldValue,  Item item, ISitecoreService service)
        {
            throw new NotImplementedException();
           
        }

        public override string SetFieldValue(object value,  ISitecoreService service)
        {
            throw new NotImplementedException();
        }

        public override Type TypeHandled
        {
            get { return typeof(FieldTypes.Image); }
        }
    }
}
