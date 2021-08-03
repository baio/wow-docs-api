module PassportRFTests

open Expecto
open ParseDoc

let Case1Words =
    [| "российская"
       "федерац"
       "и"
       "я"
       "ОТДЕЛЕНИЕМ"
       "УФМС"
       "РОССИИ"
       "по"
       "челябинской"
       "области"
       "В"
       "ГОР."
       "ТРЕХГОРНЫЙ"
       "сл"
       "23.12.2013"
       "740-049"
       "путилов"
       "максим"
       "сл"
       "александрович"
       "мух."
       "11.03.1980"
       "гор. златоуст"
       "челябинской"
       "обл."
       "PNRUSPUTILOV<3131223740049<50" |]


[<Tests>]
let tests =
    let resources = loadResources ()

    testList
        "PassportRF"
        [ testCase
              "case 1"
              (fun _ ->
                  let words = Case1Words |> cleanLine

                  let actual = PassportRF.parse resources.RuNames words

                  printfn "%O" actual
                  Expect.equal actual actual "kek") ]
