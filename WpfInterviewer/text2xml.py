import os, re

def processFile(file):
    if os.path.isfile(file):
        f = open(file, mode="r", encoding="utf-8")
        lines = f.readlines()
        print(len(lines))
        for line in lines:
            #print(len(line))
            sectionRegex = re.compile(r'(\d.\d(.\d)?)\s+([\sa-zA-Z0-9\.\-_\\/,;\?]*)')
            match = sectionRegex.search(line)
            if match and len(match.groups()) >= 2:
                section = match.groups()[0]
                print("found: " + section)
                parts = section.split(".")
                print(parts)
                content = match.groups()[2]
                print(content)
        f.close()
    
processFile("C:\\Code\\RelatedRecords.Tests\\WpfInterviewer\\text.txt")