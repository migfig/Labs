import os, re, sys

#1.0	Framework Fundamentals
#1.1	Managed vs non managed
#1.1.1	What is a managed framework and what is a non-managed framework?
dict = {} #{ '1.0', 'Framework Fundamentals' }

lineRegex = re.compile(r'(\d{1,3}.\d{1,3}(.\d{1,3})?)\s+([\sa-zA-Z0-9\.\-_\\/,;\?#\'\+\<\>\[\]\(\)\$’“”]*)')

def handleLine(line):
    match = lineRegex.search(line)
    if match and len(match.groups()) >= 2:
        section = match.groups()[0]
        if len(section.split(".")) == 2:
            section += ".0"
        #print("found: " + section)
        content = match.groups()[2]
        #print(content)
        dict[section] = content.replace("\n", "").replace("<","&lt;").replace(">","&gt;")        

def processFile(file):
    if os.path.isfile(file):
        f = open(file, mode="r", encoding="utf-8")
        lines = f.readlines()
        #print(len(lines))
        for line in lines:
            #print(len(line))
            handleLine(line)
        f.close()

def writeXml(file, content):
    if os.path.exists(file):
        os.remove(file)
    f = open(file, mode="w", encoding="utf-8")
    f.write(content)
    f.close()

def generateXml(file):
    processFile(file)
    xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><configuration><platform name=\"Net Framework\">"
    areaStarted = False
    for k in sorted(dict.keys()):
        #print(k, dict[k])
        parts = k.split(".")
        kbarea = int(parts[0])
        area = int(parts[1])
        question = int(parts[2])
        if area == 0:
            if len(xml) > 85:
                xml += "</area></knowledgeArea>"
            xml += "<knowledgeArea name=\"" + dict[k] + "\">"
            areaStarted = False
        elif question == 0:
            if areaStarted:
                xml += "</area>"
                areaStarted = False
            xml += "<area name=\"" + dict[k] + "\">"
            areaStarted = True 
        else:
            xml += "<question value=\"1\" level=\"1\">"+ dict[k] +"</question>"
    xml += "</area>"
    xml += "</knowledgeArea>"
    xml += "</platform>"
    xml += "</configuration>"
    writeXml(file.replace(".txt", ".xml"), xml)

#if __name__ == "__main__":
#    generateXml(sys.argv[1])
#else:    
generateXml("C:\\Code\\RelatedRecords.Tests\\PythonLabs\\text.txt")
