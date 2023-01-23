/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Channels;

namespace InboundXmlResponse
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public partial class Envelope
    {
        public static Envelope ReadFromXml(string xml)
        {
            var serializer = new XmlSerializer(typeof(Envelope));
            using (var reader = new StringReader(xml))
            {
                return (Envelope)serializer.Deserialize(reader);
            }
        }

        private object headerField;

        private EnvelopeBody bodyField;

        /// <remarks/>
        public object Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        /// <remarks/>
        public EnvelopeBody Body
        {
            get
            {
                return this.bodyField;
            }
            set
            {
                this.bodyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class EnvelopeBody
    {

        private EnvelopeBodyFault faultField;

        /// <remarks/>
        public EnvelopeBodyFault Fault
        {
            get
            {
                return this.faultField;
            }
            set
            {
                this.faultField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class EnvelopeBodyFault
    {

        private string faultcodeField;

        private faultstring faultstringField;

        private detail detailField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public string faultcode
        {
            get
            {
                return this.faultcodeField;
            }
            set
            {
                this.faultcodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public faultstring faultstring
        {
            get
            {
                return this.faultstringField;
            }
            set
            {
                this.faultstringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public detail detail
        {
            get
            {
                return this.detailField;
            }
            set
            {
                this.detailField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class faultstring
    {

        private string langField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class detail
    {

        private StandardFaultMessage standardFaultMessageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://sap.com/xi/AP/Common/Global")]
        public StandardFaultMessage StandardFaultMessage
        {
            get
            {
                return this.standardFaultMessageField;
            }
            set
            {
                this.standardFaultMessageField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://sap.com/xi/AP/Common/Global")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://sap.com/xi/AP/Common/Global", IsNullable = false)]
    public partial class StandardFaultMessage
    {

        private standard standardField;

        private object additionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public standard standard
        {
            get
            {
                return this.standardField;
            }
            set
            {
                this.standardField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public object addition
        {
            get
            {
                return this.additionField;
            }
            set
            {
                this.additionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class standard
    {

        private string faultTextField;

        private string faultUrlField;

        private standardFaultDetail[] faultDetailField;

        /// <remarks/>
        public string faultText
        {
            get
            {
                return this.faultTextField;
            }
            set
            {
                this.faultTextField = value;
            }
        }

        /// <remarks/>
        public string faultUrl
        {
            get
            {
                return this.faultUrlField;
            }
            set
            {
                this.faultUrlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("faultDetail")]
        public standardFaultDetail[] faultDetail
        {
            get
            {
                return this.faultDetailField;
            }
            set
            {
                this.faultDetailField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class standardFaultDetail
    {

        private string severityField;

        private string textField;

        private string idField;

        /// <remarks/>
        public string severity
        {
            get
            {
                return this.severityField;
            }
            set
            {
                this.severityField = value;
            }
        }

        /// <remarks/>
        public string text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public string id
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
    }













    

}
