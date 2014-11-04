local nvltestlib		= require "testlib"
local nvlcorelib		= require "corelib"

local function set_variable(key, value)
	if type(value) == "string" then
		nvltestlib.set_variable(key, value, "String")
		return 1
	elseif type(value) == "number" then
		-- Check if int
		if math.floor(value) == value then
			nvltestlib.set_variable(key, value, "Integer")
		-- Nope, it's a double
		else
			nvltestlib.set_variable(key, value, "Double")
		end
		return 1
	elseif type(value) == "boolean" then
		nvltestlib.set_variable(key, value, "Flag")
		return 1
	else
		return -1
	end
end

local function get_variable(key)
	return nvltestlib.get_variable(key)
end

--[[
local function say(input)
	name_and_dialogue = string.split(input,":",2);
	if table.getn(name_and_dialogue) == 1 then
		name = "";
		dialogue = input;
	elseif table.getn(name_and_dialogue) == 2 then
		name = name_and_dialogue[1];
		dialogue = name_and_dialogue[2];
	end
	return nvlcorelib.say(name, dialogue);
end
--]]

local function show_bg(alias)
	return nvlcorelib.show_bg(alias)
end

return
{
	set_variable 	= set_variable,
	get_variable	= get_variable,
	say				= say
}