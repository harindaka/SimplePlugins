def FriendlyName():
    return 'Sample Python Plugin'

def ExecutionMode():
    return plugin.ExecutionModes.MultiInstance

def Main(args):
	args.Add('ReturnValue', 'Success')
	return args

def OnAbort():
    return
