string src = "web.config";
string transformFile = "web.release.config";
string destFile = "web.transformed.config";

//execute
var document = new Microsoft.Web.XmlTransform.XmlTransformableDocument();
document.PreserveWhitespace = true;
document.Load(src);

var transform = new Microsoft.Web.XmlTransform.XmlTransformation(transformFile);

bool succeed = transform.Apply(document);

if (!succeed)
    throw new Exception("Transformation failed");

document.Save(destFile);


//sanity verify the content is right, (xml was transformed)
string content = File.ReadAllText(destFile);
var test = content.Contains("debug=\"true\"", StringComparison.OrdinalIgnoreCase);

if (test)
    throw new Exception("Transformation failed");

var lines = new List<string>(File.ReadLines(destFile));
//sanity verify the line format is not lost (otherwise we will have only one long line)
if (lines.Count <= 10)
    throw new Exception("Transformation failed");

//be nice 
transform.Dispose();
document.Dispose();
