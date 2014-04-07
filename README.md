#SimpleConfigEditor
Small .net library to create and modify config files, based on XML format.

##Usage:
to use this library we have to create new instance: 

`SimpleConfigEditor.ConfigEditor cfg = new SimpleConfigEditor.ConfigEditor('path_to_file');`

after creating instance we can use `Set` and `Get` methods to set\get a key in config file.

####Set:
This method set a `value` to a `key`.

`cfg.Set("KEY", "VALUE");`

####Get:
This method get `value` of the `key`, otherwise returns an empty string.

`cfg.Get("Key");`

####Get with default value:
This method returns `value` if exists, otherwise returns `"defaultValue"`.

`cfg.Get("KEY", "DefaultValue");`

####Delete:
This method `delete` a `key` from XML config file.

`cfg.Delete("KEY");`

####AllKeys:
This readonly property gets the list of all available `keys`  in config file as `String[]`.

`string[] keyArray = cfg.AllKeys;`
