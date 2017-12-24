set fs  = CreateObject("Scripting.FileSystemObject")
set ws  = WScript.CreateObject("WScript.Shell")
set arg = Wscript.Arguments

linkFile = arg(0)

set link = ws.CreateShortcut(linkFile)
    link.TargetPath = fs.BuildPath(arg(1), arg(2))
	link.WorkingDirectory  = arg(1)
    link.Save