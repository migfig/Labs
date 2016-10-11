
namespace Reflector.Draw.io
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class mxGraphModel
    {

        private mxGraphModelMxCell[] rootField;

        private byte gridField;

        private byte mathField;

        private string backgroundField;

        private ushort pageHeightField;

        private ushort pageWidthField;

        private decimal pageScaleField;

        private byte pageField;

        private byte foldField;

        private byte arrowsField;

        private byte connectField;

        private byte tooltipsField;

        private byte guidesField;

        private byte gridSizeField;

        private ushort dyField;

        private ushort dxField;

        private byte shadowField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("mxCell", IsNullable = false)]
        public mxGraphModelMxCell[] root
        {
            get
            {
                return this.rootField;
            }
            set
            {
                this.rootField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte grid
        {
            get
            {
                return this.gridField;
            }
            set
            {
                this.gridField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte math
        {
            get
            {
                return this.mathField;
            }
            set
            {
                this.mathField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string background
        {
            get
            {
                return this.backgroundField;
            }
            set
            {
                this.backgroundField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort pageHeight
        {
            get
            {
                return this.pageHeightField;
            }
            set
            {
                this.pageHeightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort pageWidth
        {
            get
            {
                return this.pageWidthField;
            }
            set
            {
                this.pageWidthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal pageScale
        {
            get
            {
                return this.pageScaleField;
            }
            set
            {
                this.pageScaleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte page
        {
            get
            {
                return this.pageField;
            }
            set
            {
                this.pageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte fold
        {
            get
            {
                return this.foldField;
            }
            set
            {
                this.foldField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte arrows
        {
            get
            {
                return this.arrowsField;
            }
            set
            {
                this.arrowsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte connect
        {
            get
            {
                return this.connectField;
            }
            set
            {
                this.connectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte tooltips
        {
            get
            {
                return this.tooltipsField;
            }
            set
            {
                this.tooltipsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte guides
        {
            get
            {
                return this.guidesField;
            }
            set
            {
                this.guidesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte gridSize
        {
            get
            {
                return this.gridSizeField;
            }
            set
            {
                this.gridSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort dy
        {
            get
            {
                return this.dyField;
            }
            set
            {
                this.dyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort dx
        {
            get
            {
                return this.dxField;
            }
            set
            {
                this.dxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte shadow
        {
            get
            {
                return this.shadowField;
            }
            set
            {
                this.shadowField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class mxGraphModelMxCell
    {

        private mxGraphModelMxCellMxGeometry mxGeometryField;

        private byte idField;

        private string styleField;

        private byte parentField;

        private bool parentFieldSpecified;

        private string valueField;

        private byte vertexField;

        private bool vertexFieldSpecified;

        private byte edgeField;

        private bool edgeFieldSpecified;

        private byte targetField;

        private bool targetFieldSpecified;

        private byte sourceField;

        private bool sourceFieldSpecified;

        /// <remarks/>
        public mxGraphModelMxCellMxGeometry mxGeometry
        {
            get
            {
                return this.mxGeometryField;
            }
            set
            {
                this.mxGeometryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string style
        {
            get
            {
                return this.styleField;
            }
            set
            {
                this.styleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte parent
        {
            get
            {
                return this.parentField;
            }
            set
            {
                this.parentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool parentSpecified
        {
            get
            {
                return this.parentFieldSpecified;
            }
            set
            {
                this.parentFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte vertex
        {
            get
            {
                return this.vertexField;
            }
            set
            {
                this.vertexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool vertexSpecified
        {
            get
            {
                return this.vertexFieldSpecified;
            }
            set
            {
                this.vertexFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte edge
        {
            get
            {
                return this.edgeField;
            }
            set
            {
                this.edgeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool edgeSpecified
        {
            get
            {
                return this.edgeFieldSpecified;
            }
            set
            {
                this.edgeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte target
        {
            get
            {
                return this.targetField;
            }
            set
            {
                this.targetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool targetSpecified
        {
            get
            {
                return this.targetFieldSpecified;
            }
            set
            {
                this.targetFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sourceSpecified
        {
            get
            {
                return this.sourceFieldSpecified;
            }
            set
            {
                this.sourceFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class mxGraphModelMxCellMxGeometry
    {

        private mxGraphModelMxCellMxGeometryMxPoint[] mxPointField;

        private mxGraphModelMxCellMxGeometryArray arrayField;

        private string asField;

        private decimal heightField;

        private decimal widthField;

        private decimal yField;

        private bool yFieldSpecified;

        private decimal xField;

        private bool xFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("mxPoint")]
        public mxGraphModelMxCellMxGeometryMxPoint[] mxPoint
        {
            get
            {
                return this.mxPointField;
            }
            set
            {
                this.mxPointField = value;
            }
        }

        /// <remarks/>
        public mxGraphModelMxCellMxGeometryArray Array
        {
            get
            {
                return this.arrayField;
            }
            set
            {
                this.arrayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @as
        {
            get
            {
                return this.asField;
            }
            set
            {
                this.asField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ySpecified
        {
            get
            {
                return this.yFieldSpecified;
            }
            set
            {
                this.yFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool xSpecified
        {
            get
            {
                return this.xFieldSpecified;
            }
            set
            {
                this.xFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class mxGraphModelMxCellMxGeometryMxPoint
    {

        private string asField;

        private decimal yField;

        private decimal xField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @as
        {
            get
            {
                return this.asField;
            }
            set
            {
                this.asField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class mxGraphModelMxCellMxGeometryArray
    {

        private mxGraphModelMxCellMxGeometryArrayMxPoint mxPointField;

        private string asField;

        /// <remarks/>
        public mxGraphModelMxCellMxGeometryArrayMxPoint mxPoint
        {
            get
            {
                return this.mxPointField;
            }
            set
            {
                this.mxPointField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @as
        {
            get
            {
                return this.asField;
            }
            set
            {
                this.asField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class mxGraphModelMxCellMxGeometryArrayMxPoint
    {

        private ushort yField;

        private decimal xField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }
    }


}
