-----------------------------------------------------------------------------
--  Lua wrapper for Metis.NVL functionality.
--
--  (c) 2014 Project BC Ltd.
--
--  Unauthorized copying of this file via any medium is strictly prohibited.
--	Proprietary and confidential.
--
--	Author: Bishop Myers
-----------------------------------------------------------------------------
nvl	        = require "corelib"
--audio		= require "lib.vsa.audio"

function enter(name, tag)
	nvl.show_portrait(name, tag or "")
end

function exit(name)
	nvl.exit_portrait(name)
end

function wait(seconds)
	nvl.wait(seconds or 1.0)
end

function menu(choice1, choice2, choice3, choice4)
	number_of_choices = 0
	if choice1 then number_of_choices = number_of_choices + 1 end
	if choice2 then number_of_choices = number_of_choices + 1 end
	if choice3 then number_of_choices = number_of_choices + 1 end
	if choice4 then number_of_choices = number_of_choices + 1 end
	nvl.say("# TODO: Menu handling, automatically choosing option 1")
	return 1
	--return nvl.menu(number_of_choices, choice1, choice2, choice3, choice4)
end

function exit_all()
	nvl.exit_all()
end

function replace(set1, set2, tag)
	exit(set1)
	enter(set2, tag)
end

function change(set, tag)
	enter(set, tag)
end

function fade_in(time_in_seconds)
	time_in_seconds = time_in_seconds or 1.0
	--nvl.say("# TODO: Fade in handling")
	--nvl.fade_in(time_in_seconds)
end

function fade_out(time_in_seconds)
	time_in_seconds = time_in_seconds or 1.0
	--nvl.say("# TODO: Fade out handling")
	--nvl.fade_out(time_in_seconds)
end

return 1