#! /bin/bash

targetDir=../
protocolDir=proto

cd $protocolDir
echo "Start Generate .cs file by .proto"
for file in *.proto   
do
	if test -f $file; then	
        echo "${file%%.*}"
		./protoc --csharp_out=$targetDir  $file
	fi
done

echo "Success !" 
