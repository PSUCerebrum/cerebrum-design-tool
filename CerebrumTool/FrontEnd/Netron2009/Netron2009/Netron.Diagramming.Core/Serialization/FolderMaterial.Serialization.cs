using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Xml.Schema;
namespace Netron.Diagramming.Core
{
    /// <summary>
    /// Complementary partial class related to (de)serialization.
    /// </summary>
   [Serializable]
    public partial class FolderMaterial : ISerializable, IXmlSerializable, IDeserializationCallback
    {
        #region Deserialization constructor
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected FolderMaterial(SerializationInfo info, StreamingContext context)            
        {
            if(Tracing.BinaryDeserializationSwitch.Enabled)
                Trace.WriteLine("Deserializing the fields of 'FolderMaterial'.");

            double version = info.GetDouble("FolderMaterialVersion");
            
            this.Gliding = info.GetBoolean("Gliding");
            this.Resizable = info.GetBoolean("Resizable");            
            this.Visible = info.GetBoolean("Visible");
            header = info.GetValue("Header", typeof(ClickableLabelMaterial)) as ClickableLabelMaterial;
            plusminus = info.GetValue("PlusMinus", typeof(SwitchIconMaterial)) as SwitchIconMaterial;
            mEntries = info.GetValue("Entries", typeof(CollectionBase<IShapeMaterial>)) as CollectionBase<IShapeMaterial>;
            mEntries.OnItemAdded += new EventHandler<CollectionEventArgs<IShapeMaterial>>(mEntries_OnItemAdded);
            Rectangle = (Rectangle) info.GetValue("Rectangle", typeof(Rectangle));
            
            mCollapsed = info.GetBoolean("Collapsed");
        }
        #endregion

        #region Serialization events
       /*
        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            Trace.Indent();
            Trace.WriteLine("Starting to serializing the 'FolderMaterial' class...");
        }
        [OnSerialized]
        void OnSerialized(StreamingContext context)
        {
            Trace.WriteLine("...serialization of 'FolderMaterial' finished");
            Trace.Unindent();
        }
        */
        #endregion

        #region Deserialization events
       /*
        [OnDeserializing]      
        void OnDeserializing(StreamingContext context)
        {
            Trace.Indent();
            Trace.WriteLine("IDeserializationCallback of 'FolderMaterial' called.");
        }
        * */
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
         {
             if(Tracing.BinaryDeserializationSwitch.Enabled)
                Trace.WriteLine("...deserialization of 'FolderMaterial' finished");
             foreach(IShapeMaterial material in mEntries)
             {
                 material.Shape = this.Shape;                 
             }
             plusminus.OnExpand += new EventHandler(plusminus_OnExpand);
             plusminus.OnCollapse += new EventHandler(plusminus_OnCollapse);
             plusminus.Collapsed = mCollapsed;//this will call the Expand/Collapse of the folder via the event in the lines above            
            
            Transform(Rectangle);     
            
         }
       
        #endregion

        #region Serialization
         /// <summary>
         /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> with the data needed to serialize the target object.
         /// </summary>
         /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> to populate with data.</param>
         /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"></see>) for this serialization.</param>
         /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public override void GetObjectData(SerializationInfo info, StreamingContext context) 
        {
            if(Tracing.BinarySerializationSwitch.Enabled)
                Trace.WriteLine("Serializing the fields of 'FolderMaterial'.");
            base.GetObjectData(info, context);

            info.AddValue("FolderMaterialVersion", folderMaterialVersion);
            info.AddValue("Header", this.header, typeof(ClickableLabelMaterial));
            info.AddValue("PlusMinus", this.plusminus, typeof(SwitchIconMaterial));
            info.AddValue("Entries", this.mEntries, typeof(CollectionBase<IShapeMaterial>));
            info.AddValue("Collapsed", mCollapsed);
        }
        #endregion

        #region Xml serialization
        /// <summary>
        /// This property is reserved, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"></see> to the class instead.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"></see> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"></see> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"></see> method.
        /// </returns>
        public override XmlSchema GetSchema()
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"></see> stream from which the object is deserialized.</param>
        public override void ReadXml(System.Xml.XmlReader reader)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"></see> stream to which the object is serialized.</param>
        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }
        #endregion



        /// <summary>
        /// Runs when the entire object graph has been deserialized.
        /// </summary>
        /// <param name="sender">The object that initiated the callback. The functionality for this parameter is not currently implemented.</param>
        public override void  OnDeserialization(object sender)
        {
            base.OnDeserialization(sender);
            if(Tracing.BinaryDeserializationSwitch.Enabled)
                Trace.WriteLine("IDeserializationCallback of 'FolderMaterial' called.");
            

        }
    }
}
