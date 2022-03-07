## WabiSabi Coinjoins dumper

This tool is used to dump all the coinjoins.

## How to run it

### First, you need to have the list of wabisabi coinjoin ids.

The coordinator stores the tx is and you can get them by doing:

```bash
ssh testing -t 'cat ~/.walletwasabi/backend/WabiSabi/CoinJoinIdStore.txt' > coinjoins-testnet.txt
```

### Run it

```bash
dotnet run  > testnet.txt
```

### Results

You should be able to see something like the following:

```
05/03/22 01:54:55, d0083145fa20e41bd898b3cdcb3287b7cd5c2fc2f5c0259b9c570559ca3ba5d4,tb1qq03y5c4cvhdpzxr344xml30x6jckmcdtuxepty, 1.29140163
05/03/22 01:54:55, d0083145fa20e41bd898b3cdcb3287b7cd5c2fc2f5c0259b9c570559ca3ba5d4, tb1q7gv8sjfha0rspw6uzuhs63df2hxrs4l2fz9z0j, 0.2
05/03/22 01:54:55, d0083145fa20e41bd898b3cdcb3287b7cd5c2fc2f5c0259b9c570559ca3ba5d4, tb1qw3ezsp7f4j74px5242w0xx66nea7vyj9r0cjhn, 0.08388608
```

## Note:

This is a poor-man solution that uses a block explorer. It can be modified to use the RPC interface if you have access to a node.

## How to process the file

If well it can be improted in Excel or some other spreadsheet, it is more flexible to use bash or f#. For example, this is how you can parse the file and get the frequency distribution of standard values used by WabiSabi

```bash
$ dotnet fsi

open System.IO;;

File.ReadAllLines("all.txt")
|> Array.map (fun x -> x.Split(", "));;
|> Array.map (fun x -> x[3])
|> Seq.countBy id
|> Seq.sortByDescending snd;;
```

**Output**
```
  [("0.00005", 3907); ("0.0001", 2427); ("0.00008192", 2398); ("0.0002", 2311);
   ("0.00006561", 1921); ("0.00039366", 1561); ("0.00059049", 1324);
   ("0.00065536", 1249); ("0.00032768", 1235); ("0.00019683", 1095);
   ("0.00016384", 1085); ("0.0005", 1079); ("0.00013122", 1023);
   ("0.00131072", 818); ("0.00354294", 800); ("0.00262144", 777);
   ("0.002", 776); ("0.001", 745); ("0.00177147", 589); ("0.00118098", 573);
   ("0.00531441", 524); ("0.01062882", 492); ("0.00524288", 467);
   ("0.01048576", 350); ("0.005", 326); ("0.02097152", 312);
   ("0.03188646", 272); ("0.01", 255); ("0.01594323", 236); ("0.02", 229);
   ("0.05", 197); ("0.04194304", 181); ("0.08388608", 148);
   ("0.04782969", 139); ("0.1", 123); ("0.09565938", 116); ("0.14348907", 98);
   ("0.16777216", 96); ("0.2", 62); ("0.28697814", 41); ("0.33554432", 36);
   ("0.43046721", 19); ("0.00006299", 12); ("0.00006122", 12); ("0.5", 11);
   ("0.00196132", 9); ("0.00009738", 9); ("0.00059524", 9); ("0.00005883", 8);
   ("0.67108864", 7); ("0.00005984", 7); ("0.00199662", 7); ("0.01062558", 7);
   ("0.00176671", 6); ("0.00006062", 6); ("0.01005396", 6); ("0.01062406", 6);
   ("0.02010992", 6); ("0.00006216", 5); ("0.00005922", 5); ("0.00189645", 5);
   ("0.00009421", 5); ("0.00005901", 5); ("0.06376954", 5); ("0.00006554", 5);
   ("0.00032506", 5); ("0.00005604", 5); ("0.00261882", 5); ("0.86093442", 4);
   ("0.00006317", 4); ("0.000062", 4); ("0.00242069", 4); ("0.00785956", 4);
   ("0.00230596", 4); ("0.00479628", 4); ("0.00006478", 4); ("0.00114124", 4);
   ("0.0039274", 4); ("0.00065198", 4); ("0.03222522", 4); ("0.04856356", 4);
   ("0.00006083", 4); ("0.00006237", 4); ("0.00006453", 4); ("0.00006145", 4);
   ("0.00019538", 4); ("0.00005936", 4); ("0.00006037", 4); ("0.00072292", 4);
   ("0.00006212", 4); ("0.0035397", 3); ("0.00339664", 3); ("0.00468097", 3);
   ("0.00099738", 3); ("0.00104426", 3); ("0.00061736", 3); ("0.00255043", 3);
   ("0.00005782", 3); ("0.00017636", 3); ("0.00006078", 3); ...]
   ```