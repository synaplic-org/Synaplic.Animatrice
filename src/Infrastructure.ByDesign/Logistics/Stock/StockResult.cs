﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Infrastructure.ByDesign.Logistics.Stock
{
    // StockResponse myDeserializedClass = JsonSerializer.Deserialize<StockResponse>(myJsonResponse);
    public class D
    {
        public List<Result> results { get; set; }
    }

    public class Metadata
    {
        public string uri { get; set; }
        public string type { get; set; }
    }

    public class Result
    {
        public Metadata __metadata { get; set; }
        public string CCATCP_UUID { get; set; }
        public string CCUSTODIAN_UUID { get; set; }
        public string CINV_TYPE { get; set; }
        public string CINV_UNIT { get; set; }
        public bool CIN_CONSIGNMENT_IND { get; set; }
        public bool CIN_QUALITY_IND { get; set; }
        public string CISTOCK_UUID { get; set; }
        public string CLOCATION_UUID { get; set; }
        public string CLOG_AREA_UUID { get; set; }
        public string CMATERIAL_UUID { get; set; }
        public string COWNER_UUID { get; set; }
        public string CPARENT_LU_UUID { get; set; }
        public string CPRS_VER_UUID { get; set; }
        public bool CRESTRICTED { get; set; }
        public string CSITE_UUID { get; set; }
        public string CSPA_UUID { get; set; }
        public string FCENDING_QUANTITY { get; set; }
        public string FCENDING_QUANTITY_NODIM { get; set; }
        public string KCENDING_QUANTITY { get; set; }
        public string KCENDING_QUANTITY_NODIM { get; set; }
        public string TCATCP_UUID { get; set; }
        public string TCUSTODIAN_UUID { get; set; }
        public string TINV_TYPE { get; set; }
        public string TINV_UNIT { get; set; }
        public string TISTOCK_UUID { get; set; }
        public string TLOCATION_UUID { get; set; }
        public string TLOG_AREA_UUID { get; set; }
        public string TMATERIAL_UUID { get; set; }
        public string TOWNER_UUID { get; set; }
        public string TPARENT_LU_UUID { get; set; }
        public string TPRS_VER_UUID { get; set; }
        public string TSITE_UUID { get; set; }
    }

    public class StockResponse
    {
        [JsonPropertyName("d")]
        public D D { get; set; }
    }


}
