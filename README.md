branch master: allows to pack/unpack staticdata

branch sound: allows to pack/unpack sounds

usage: open csproj, compile and run, select a file, and press a a button

dat/json files generate in the same folder as the input file

modifying the data structure (advanced users only):
- modify the .proto file
- run generate.bat
- reopen the project
- compile and run

dependencies:
- google protobuf
- newtonsoft json
