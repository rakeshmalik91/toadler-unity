rm encrypted_private_key_path

java ^
 -Djava.security.properties=extra.security ^
 -cp "bcprov-jdk18on-1.83.jar;pepk.jar" ^
 com.google.wireless.android.vending.developer.signing.tools.extern.export.ExportEncryptedPrivateKeyTool ^
 --keystore=../../../RakeshMalik2024.keystore ^
 --alias=toadler ^
 --output=encrypted_private_key_path ^
 --rsa-aes-encryption ^
 --include-cert ^
 --encryption-key-path=encryption_public_key.pem