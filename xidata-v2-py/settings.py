# noesis.py - Settings for Noesis tool integration
noesis_title = "Noesis"
noesis_path = "E:\\FF11 Tools\\3D - Noesis\\Noesis64.exe"
noesis_args = "-ff11bumpdir normals -ff11noshiny -ff11hton 16 -ff11optimizegeo -ff11keepnames " \
              "-fbxtexrelonly -fbxtexext .png -rotate 180 0 0 -scale 1"

# The path to the FFXI installation directory
ffxi_path = "D:\\catseyexi\\catseyexi-client\\Game\\FINAL FANTASY XI"

# The path where extracted files will be saved
extract_output_path = "E:\\FF11 Assets\\xi-data\\xidata-v2-py\\extracted"

# Whether to extract weapons with skeletons. Set true if you want skeletal weapons.
extract_weapons_with_skeleton = False

# Whether to overwrite existing files during extraction
extract_overwrite_existing = False

# send key options
send_key_delay = 0.1
send_key_loop_delay = 0.01
send_text_delay = 0.01

# If enabled, debug messages will be printed for key presses
enable_send_key_debug = False