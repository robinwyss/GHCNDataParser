namespace GHCNDataParser

open System.IO
open Newtonsoft.Json

module StationParser = 

    type Station = {
        stationId: string; 
        name: string; 
        state: string;
        latitude: float; 
        longitude: float; 
        elevation: float
        } 
    let stationToJson station = 
        JsonConvert.SerializeObject(station)
    
    let parseStations (lines:seq<string>) =
        lines |> Seq.map (fun line ->
                { 
                    stationId = line.[0..10];
                    latitude = line.[12..19] |> float;
                    longitude = line.[21..29] |> float;
                    elevation = line.[31..36] |> float;
                    state = line.[38..39];
                    name = (line.[40..70]).Trim();
                }
            )

    let convertStationFileToJson filePath =
        let lines = File.ReadAllLines(filePath)
        let stations = parseStations lines
        File.Delete("stations.json")
        File.WriteAllText("stations.json", JsonConvert.SerializeObject(stations))