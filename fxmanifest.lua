fx_version 'cerulean'
game 'gta5'

server_scripts {
    "Server/bin/Release/*.net.dll",
}

client_scripts {
    "Client/bin/Release/*.net.dll",
}

exports {
    "DrawText3D"
}

mono_rt2 'Prerelease expiring 2023-06-30. See https://aka.cfx.re/mono-rt2-preview for info.'