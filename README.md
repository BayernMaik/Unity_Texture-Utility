## Included Editor Tools
### Channel Packer
![Channel Packer](./Documentation/ChannelPackerPreview.JPG "Channel Packer")
### _Description:_<br>
The Channel-Packer Editor Tool allows you to quickly Pack Color Channel Data from multiply source Textures into a new Texture Asset<br>
This Tool is Hardware accelerated using a Compute Shader (super duper fast since every pixel works in parallel on its own Thread on the GPU)<br>

<br>

### <b> _Usage_ </b>
- Open the Editor Window: (Unity Toolbar) -> Tools -> Channel Packer (Texture2D)<br> 
- In the left "Layer" Scroll-View, add Textures you want to use to create a new Texture Asset<br>
- Select the Source Textures desired Color Channel & choose the Target Texture Channel popping up below<br>
- Set Target Texture Resolution, Texture Format and File Format
- Enter Texture Save Path/Name (default "Assets/Texture2D)
- Click "Save" (Use Unity's Texture Import Options for further configuration)

### <b> _Changelog_ </b>
- V0.0.1 Alpha: Initial Release

### <b> _Known Issues_ </b>
- Limited Format Options
- Doesn't accommodate "User-Error"<br>
(Source Texture Layer Resolution doesnt rescale to Target Resolution)<br>
(Errors from mismatching Texture Formats and File Formats aren't cought)<br>