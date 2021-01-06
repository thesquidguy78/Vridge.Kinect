# Vridge.Kinect.Advanced

This is a Kinect driver for Vridge that send data for head, left and right hand. The driver works with Kinect for Xbox One and Xbox 360.
It's possible to send specific data (only head, left position, right rotation...). This driver only works on x64 systems.

This fork provides some quality-of-life changes for the original driver.

## Changes in this Fork
* If "Send Left Rotation" or "Send Right Rotation" is disabled, the controllers will face the orientation of the headset.

* "Upright Hands" can be enabled in when one of these options is enabled. However, it only really works for games that don't require full 360-degree tracking (Beat Saber, Elite Dangerous).
    * On that note, the Kinect tracking is very finicky when your back is facing the sensor. Your hands may swap and tracking may begin to lose reliability.

* Added Xinput controller support via SharpDX. The source includes the SharpDX implementation if you wish to change controller mappings (see GamepadSys.cs)

* Default Controller Bindings (as mapped to an Xbox 360/One/Series Controller)
<p><a target="_blank" rel="noopener noreferrer" href="/thesquidguy78/Vridge.Kinect/blob/master/mapping.png"><img src="/thesquidguy78/Vridge.Kinect/raw/master/mapping.png" alt="Example Screenshot" style="max-width:100%;"></a></p>

## License
This project is released under the MIT License.
