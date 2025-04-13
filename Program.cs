// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Serialization;

class Covid
{
  [JsonPropertyName("satuan_suhu")]
  public string SatuanSuhu { get; set; }

  [JsonPropertyName("batas_hari_demam")]
  public long BatasHariDemam { get; set; }

  [JsonPropertyName("pesan_ditolak")]
  public string PesanDitolak { get; set; }

  [JsonPropertyName("pesan_diterima")]
  public string PesanDiterima { get; set; }

  public Covid(string suhu, long batasHari, string ditolak, string diterima) {
    SatuanSuhu = suhu;
    BatasHariDemam = batasHari;
    PesanDitolak = ditolak;
    PesanDiterima = diterima;
  }
}

class CovidConfig {
  string filePath = "covid_config.json";
  public Covid covid;

  public CovidConfig() {
    string jsonString = File.ReadAllText(filePath);
    try {
      covid = JsonSerializer.Deserialize<Covid>(jsonString);
    } catch (Exception) {
      covid = new Covid( "celcius", 14, "Anda tidak diperbolehkan masuk ke dalam gedung ini", "Anda dipersilahkan untuk masuk ke dalam gedung ini");
      JsonSerializerOptions opt = new JsonSerializerOptions() {
        WriteIndented = true
      };
      string jsonSerialized = JsonSerializer.Serialize(covid, opt);
      File.WriteAllText(filePath, jsonSerialized);
    }
  }

  public void UbahSatuan() {
    if (covid.SatuanSuhu == "celcius") {
      covid.SatuanSuhu = "fahrenheit";
    } else {
      covid.SatuanSuhu = "celcius";
    }
  }
}

class Program {
  public static void Main(string[] args) {
    CovidConfig config = new CovidConfig();

    masukGedung(config);
    config.UbahSatuan();
    Console.WriteLine("");
    masukGedung(config);
  }

  static void masukGedung(CovidConfig config) {
    Console.WriteLine($"Berapa suhu badan anda saat ini? Dalam nilai {config.covid.SatuanSuhu}");
    double suhu = double.Parse(Console.ReadLine());

    Console.WriteLine($"Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala demam?");
    int lastDemam = int.Parse(Console.ReadLine());

    if (lastDemam <= 14) {
      Console.WriteLine(config.covid.PesanDitolak);
      return;
    }
    if(config.covid.SatuanSuhu == "celcius") {
      if (suhu < 36.5 || suhu > 37.5) {
        Console.WriteLine(config.covid.PesanDitolak);
        return;
      }
    } else {
      if (suhu < 97.7 || suhu > 99.5) {
        Console.WriteLine(config.covid.PesanDitolak);
        return;
      }
    }

    Console.WriteLine(config.covid.PesanDiterima);
  }
}
