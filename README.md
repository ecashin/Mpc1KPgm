# MPC 1000 PGM Reader

This software reads a program file
in the MPC 1000 program-file format
as documented at the web site below.

https://mybunnyhug.org/fileformats/pgm/

## Dependencies

This software depends on .NET Core 5
and has been tested on Linux.

The `jq` command is handy for working with JSON
but is not required.

## Building and Running

The software is built and run
in place,
in one step,
as shown in the example command below.

    dotnet run ./SFM-DMX/DMX\ From\ Mars\ 1.pgm | jq .

