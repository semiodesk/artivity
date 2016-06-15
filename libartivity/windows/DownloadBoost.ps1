
$file = "$pwd\boost.zip"
If (!(Test-Path $file)){
  $webclient = New-Object System.Net.WebClient
	$url = "https://sourceforge.net/projects/boost/files/boost/1.61.0/boost_1_61_0.zip"
	$webclient.DownloadFile($url,$file)

}
$target = "$pwd\boost_1_61_0\"
$dest = "$pwd\"
If (!(Test-Path $target)){
	
	$shell = new-object -com shell.application
	$zip = $shell.NameSpace($file)
	foreach($item in $zip.items())
	{
		$shell.Namespace($dest).copyhere($item)
	}
}