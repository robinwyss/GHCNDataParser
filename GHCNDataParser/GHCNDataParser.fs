namespace GHCNDataParser
open System.IO


open StationParser

type DailyMeasure = { value: int; source: string; day: int}

type MonthlyMeasure = { 
    station: string; 
    year: int; 
    month: int;
    element: string;
    values: DailyMeasure list}

module main = 
    let parseFile filePath =
        let lines = File.ReadAllLines(filePath)
        let measures = 
            lines |> Seq.map (fun line ->
                let values = [1..31] |> Seq.map (fun i -> 
                        let delta = 8 * (i-1)
                        let temp = line.[(21+delta)..(25+delta)]
                        let source = (line.[(28+delta)]).ToString()
                        {value = temp |> int; source = source; day= i}
                    ) 
                { 
                    station = line.[0..10];
                    year = line.[11..14] |> int;
                    month = line.[15..16] |> int;
                    element = line.[17..20];
                    values = values |> Seq.toList
                }
            )
        for measure in measures do 
            printf "%A" measure

       // printf "%s has %d lines\n" filePath lines.Length
        0

    let readDataFromFolder dataFolder = 
        if Directory.Exists(dataFolder) then
                for file in Directory.GetFiles(dataFolder) do
                    parseFile file


    [<EntryPoint>]
    let main argv =
        let timer = new System.Diagnostics.Stopwatch()
        timer.Start()
        if argv.Length = 0 then
            convertStationFileToJson @"C:\Users\robin\Documents\playground\GHCNDataParser\data\ghcnd-stations.txt"
            //readDataFromFolder @"C:\Users\robin\Documents\playground\GHCNDataParser\data\ghcnd_gsn"
         else
            convertStationFileToJson @""
            //readDataFromFolder argv.[0]
        timer.Stop()
        printf "done in %i milliseconds" timer.ElapsedMilliseconds
        System.Console.ReadKey() |> ignore
        0 // return an integer exit code
