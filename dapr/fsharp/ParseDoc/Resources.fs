[<AutoOpen>]
module ParseDoc.Resources

open System.IO
open System.Reflection

type Names =
    { FirstNames: string []
      LastNames: string []
      MiddleNames: string [] }

type Resources = { RuNames: Names; EnNames: Names }

let private getResourcePath = sprintf "ParseDoc.data.%s.csv"

let private readResourcePath (assembly: Assembly) path =
    use stream: Stream = assembly.GetManifestResourceStream path
    use reader = new StreamReader(stream)
    reader.ReadToEnd()

let private readResourceFile assembly name =
    let path = getResourcePath name
    readResourcePath assembly path

let private readResourceFileAsLines assembly name =
    let str = readResourceFile assembly name
    str.Split([| "\r\n"; "\r"; "\n" |], options = System.StringSplitOptions.RemoveEmptyEntries)

let loadResources () =

    let assembly = Assembly.GetExecutingAssembly()

    let fnames =
        readResourceFileAsLines assembly "fnames"

    let lnames =
        readResourceFileAsLines assembly "lnames"

    let mnames =
        readResourceFileAsLines assembly "mnames"

    let fnamesEn =
        readResourceFileAsLines assembly "fnames_en"

    let lnamesEn =
        readResourceFileAsLines assembly "lnames_en"

    let mnamesEn =
        readResourceFileAsLines assembly "mnames_en"

    { RuNames =
          { FirstNames = fnames
            LastNames = lnames
            MiddleNames = mnames }
      EnNames =
          { FirstNames = fnamesEn
            LastNames = lnamesEn
            MiddleNames = mnamesEn }

    }
