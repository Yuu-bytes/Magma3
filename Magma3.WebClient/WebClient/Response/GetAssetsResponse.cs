using System.Text.Json.Serialization;

namespace Magma3.WebClient.WebClient.Response
{
    public class GetAssetsResponse
    {
        [JsonPropertyName("Id")]
        public string? Id { get; set; }

        [JsonPropertyName("Compliance")]
        public string? Compliance { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Type")]
        public string? Type { get; set; }

        [JsonPropertyName("Subtype")]
        public string? Subtype { get; set; }

        [JsonPropertyName("HelperId")]
        public string? HelperId { get; set; }

        [JsonPropertyName("HeritageAsset")]
        public string? HeritageAsset { get; set; }

        [JsonPropertyName("SerialNumber")]
        public string? SerialNumber { get; set; }

        [JsonPropertyName("Model")]
        public string? Model { get; set; }

        [JsonPropertyName("Manufacturer")]
        public string? Manufacturer { get; set; }

        [JsonPropertyName("Width")]
        public decimal? Width { get; set; }

        [JsonPropertyName("Height")]
        public decimal? Height { get; set; }

        [JsonPropertyName("Inch")]
        public decimal? Inch { get; set; }

        [JsonPropertyName("Weight")]
        public decimal? Weight { get; set; }

        [JsonPropertyName("RateDepreciation")]
        public decimal? RateDepreciation { get; set; }

        [JsonPropertyName("AcquisitionDate")]
        public DateTime? AcquisitionDate { get; set; }

        [JsonPropertyName("Real.ResidualValue")]
        public decimal? RealResidualValue { get; set; }

        [JsonPropertyName("Dolar.ResidualValue")]
        public decimal? DolarResidualValue { get; set; }

        [JsonPropertyName("RealValue")]
        public decimal? RealValue { get; set; }

        [JsonPropertyName("DolarValue")]
        public decimal? DolarValue { get; set; }

        [JsonPropertyName("ManufacturingDate")]
        public DateTime? ManufacturingDate { get; set; }

        [JsonPropertyName("ValidityDate")]
        public DateTime? ValidityDate { get; set; }

        [JsonPropertyName("Maintenances")]
        public List<MaintenanceDto>? Maintenances { get; set; }

        [JsonPropertyName("AssociateAssets")]
        public string? AssociateAssets { get; set; }

        [JsonPropertyName("Observations")]
        public string? Observations { get; set; }

        [JsonPropertyName("Tag")]
        public string? Tag { get; set; }

        [JsonPropertyName("AgentDataLastCommunication")]
        public DateTime? AgentDataLastCommunication { get; set; }

        [JsonPropertyName("AgentVersion")]
        public string? AgentVersion { get; set; }

        [JsonPropertyName("AgentInstalled")]
        public string? AgentInstalled { get; set; }

        [JsonPropertyName("AgentInstalledRaw")]
        public bool? AgentInstalledRaw { get; set; }
    }

    public class MaintenanceDto
    {
        [JsonPropertyName("Id")]
        public string? Id { get; set; }

        [JsonPropertyName("TicketId")]
        public string? TicketId { get; set; }

        [JsonPropertyName("MaintenanceDate")]
        public DateTime? MaintenanceDate { get; set; }

        [JsonPropertyName("AssetName")]
        public string? AssetName { get; set; }

        [JsonPropertyName("PeriodicityDays")]
        public int? PeriodicityDays { get; set; }

        [JsonPropertyName("Type")]
        public string? Type { get; set; }

        [JsonPropertyName("Responsible")]
        public string? Responsible { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; }

        [JsonPropertyName("Value")]
        public decimal? Value { get; set; }

        [JsonPropertyName("Observations")]
        public string? Observations { get; set; }
    }
}
