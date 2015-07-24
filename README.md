# Offroadcode proudly presents it's  media sync tool.

This tool tool is a media sharing system.

#Usecase: Umbraco
In umbraco, on the live environment, an editor can be adding images and when we look at the site on a different environment, i.e. locally some of the images will be missing. This tool helps to avoid this issue by providing a method for sharing these assets amongst developers.

##Host
The host is a simple MVC Web API, which stores the assets. It can host multiple projects.

##Client
###Uploading
On the envorionment which is considered live, you can run the command "mediasync up" which will upload the media content up to your storage host. this will do a semi intelligent upload by first diffing the assets which are on the host for the project already and only uploading those which have changed or are new.
###Downloading
If you want your assets on your local machine, you can run the command "mediasync" which will download the media content from your storage host. this will do a semi intelligent download by first diffing the assets which are on the host for the project and those which are differnet on your local machine or missing.


By default the client looks for settings from a file named "project.xml" (this can be changed by using the "filename" parameter)
```
<Project>
        <MediaSync>
                <Host>http://YourHostnameHere/</Host>
                <Name>ProjectNameHere</Name>
                <!-- this is the path on your local machine to the asset directory, everything in this folder will be synced -->
                <MediaDirectory>/Build/Media</MediaDirectory>
        </MediaSync>
</Project>
```
