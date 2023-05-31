
1- Account > SignIn View'ına google ile baðlan linki oluþturulur.
	<a class="btn btn-primary" asp-controller="Account" asp-action="ExternalLogin" asp-route-provider="Google"> Google </a>

2- Google ve Identity Paketleri yüklenir.
	* Microsoft.AspNetCore.Authotication.Google
	* Microsoft.AspNetCore.Identity

3- Google API den ClientId ve ClientSecret alýnýr.
	Google Developer Console da bir proje oluþturulur.
	Kimlik doðrulama url eklenir.
	http:localhost:port/signin-google
4- Program.cs e AddAuthentication.AddGoogle option ayarý 
	AddIdentity options

5- Account Controllers giriþ için gerekli action yazýlýr.
