# Generic python-like wrappers to Chiroptera

import Chiroptera.Base
import System.Drawing
from chicore import *

__all__ = ('colorize', 'send', 'write', 'isconnected', 'receive', 'isdebug', 'run', 'addcommand', 'removecommand', 'getopts',)

def colorize(str, fg, bg=None):
	C = Chiroptera.Base.Color
	if not isinstance(fg, C):
		if fg != None:
			fg = C.FromHtml(fg)
		else:
			fg = C.Empty
		
	if not isinstance(bg, C):
		if bg != None:
			bg = C.FromHtml(bg)
		else:
			bg = C.Empty
		
	str = Chiroptera.Base.Ansi.ColorizeString(str, fg, bg)
	return str

def send(str):
	Net.SendLine(str)
	
def write(str, *args):
	"""Writes text to screen.
	
	str - string to be printed
	args - formatting args
	
	Examples:
    write("foobar")
    write("foo {0} bar {1}", 123, "pla")
	"""
	Console.WriteLine(str, *args)

def isconnected():
	return Net.IsConnected
	
def receive(str):
	Net.ReceiveLine(str)

def isdebug():
	Chi.IsDebug()

def run(file):
	Chi.RunScript(file)

def addcommand(cmd, action, help, longhelp):
	if CmdMgr.HasCommand(cmd):
		print "warning: overriding command '" + cmd + "'"
		removecommand(cmd)
	CmdMgr.AddCommand(cmd, action, help, longhelp)

def removecommand(cmd):
	CmdMgr.RemoveCommand(cmd)

def getopts(input, optstring):
	args, opts = CmdMgr.GetOpts(input, optstring)
	return (args, opts)
