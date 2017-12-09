# service-website-codfirstDB
** important >>
	Make project work on Your ofline SYS:
	1.Open nuget page Console : //write the folloing
	
		add-Migration name
		update-database
		
	* : as result connection String will be Your sys authentication.
	** : In Latest Version Not sure!
	*** : added for Toturial
	
	noted : by MORFI
		
		How Connect Throw Local LAN ? >>
		
		Use CMD :
		for Oppening a port Use Your IP : localport
		netsh http add urlacl url=http://192.168.1.42:58938/ user=everyone
		
		for adding Exeption to Fierwall
		netsh advfirewall firewall add rule name="IISExpressWeb" dir=in protocol=tcp localport=58938 profile=private remoteip=localsubnet action=allow

		Chainge IIS setting : 
		1. go to .vs folder in project files > find aplication.config open with note do as Blow :
		
		2.The Example Below…

** This is the IIS application.config

<site name="Alpha.Web" id="2">
    <application path="/">
        <virtualDirectory path="/" physicalPath="C:\Users\Johan\HgReps\Alpha\Alpha.Web" />
    </application>
    <bindings>
        <binding protocol="http" bindingInformation="*:58938:localhost" />
    </bindings>
</site>

From <https://johan.driessen.se/posts/Accessing-an-IIS-Express-site-from-a-remote-computer> 


	3. Add the Following as Continue….

<binding protocol="http" bindingInformation="*:58938:192.168.1.42" />

From <https://johan.driessen.se/posts/Accessing-an-IIS-Express-site-from-a-remote-computer> 



The res Should BE Like VVVVVV

<site name="Alpha.Web" id="2">
    <application path="/">
        <virtualDirectory path="/" physicalPath="C:\Users\Johan\HgReps\Alpha\Alpha.Web" />
    </application>
    <bindings>
        <binding protocol="http" bindingInformation="*:58938:localhost" />
	<binding protocol="http" bindingInformation="*:58938:192.168.1.42" />
    </bindings>
</site>

noted : by MORFI