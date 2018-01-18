module GHCNDataParser
open System.IO

type DailyMeasure = { value: int; source: string; day: int}

type MonthlyMeasure = { 
    station: string; 
    year: int; 
    month: int;
    element: string;
    values: DailyMeasure list}

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
    if argv.Length = 0 then
        readDataFromFolder @"C:\Users\robin\Documents\Code\GHCN\data\ghcnd_gsn\ghcnd_gsn"
     else
        readDataFromFolder argv.[0]
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
