-----------------------------------------------------------------------------
--  Provides audio functionality.
--
--  (c) 2013 Project BC Ltd.
--
--  Unauthorized copying of this file via any medium is strictly prohibited.
--	Proprietary and confidential.
--
--	Author: Bishop Myers
-----------------------------------------------------------------------------
-- NOTE: Requires nvl to already be loaded
-----------------------------------------------------------------------------
music = {}
sound = {}

music.fade_out = function(seconds)
	local time = seconds or 5
	nvl.fade_out_music(time)
end

function battle_music()
	nvl.music("Paradox Prince")
end

function get_item_sound()	
	sound("ui/melodic3_affirm")
end

function sound(sfx)
	--nvl.say("# TODO: SFX handling")
	nvl.sfx(sfx)
end