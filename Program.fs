module SqlHydraAndFSharpCore.Program

open SqlHydra.Query

let openSqlite =
    async {
        let conn = new Microsoft.Data.Sqlite.SqliteConnection("")
        do! conn.OpenAsync() |> Async.AwaitTask

        use attachDbo = conn.CreateCommand(CommandText = "attach database '' as 'dbo'")
        let _ = attachDbo.ExecuteNonQuery()

        use createSchema = conn.CreateCommand(CommandText = SqliteTestDatabase.schema)
        let _ = createSchema.ExecuteNonQuery()

        return conn
    }

let sharedSqlite db =
    let compiler = SqlKata.Compilers.SqliteCompiler()
    ContextType.Shared(new QueryContext(db, compiler)) // For more information see https://aka.ms/fsharp-console-apps

let queryWithJoin =
    async {
        use! db = openSqlite

        let ctx = sharedSqlite db

        let date = System.DateOnly(2024, 6, 20)
        let lBound = System.DateTime(2024, 6, 20, 8, 0, 0)
        let uBound = System.DateTime(2024, 6, 20, 10, 0, 0)


        let! records =
            selectAsync ctx {
                for table1 in dbo.table1 do
                    join table2 in dbo.table2 on (table1.id = table2.table1_id)
                    where (table2.date = date && lBound <= table2.timestamp && table2.timestamp <= uBound)

                    select (table1, table2)
                    toList
            }

        printfn $"Got %O{records} from the db"

        return ()
    }


[<EntryPoint>]
let main argv =
    Async.RunSynchronously
    <| async {
        do! queryWithJoin
        return 0
    }
