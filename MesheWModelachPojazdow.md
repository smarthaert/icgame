# Ale o co chodzi? #

Jako, że docelowo mamy mieć sporo różnych pojazdów jednych mniej drugich bardziej skompilowanych wypadało by opisać jakie znajdują się w nich meshe, co by ujednolicić animowanie w nic co niektórych elementów czy dodawanie do nich efektów. Jako że większość tej wiedzy znajduje się w mailach na grupie dyskusyjnej (której nikt nie czyta) należało ją stamtąd uratować - a przy okazji stworzę sobie ściągawkę do przyszłych modeli, tak żeby przyszłościowo wszystko się zgadzało.

# Meshe wspólne #

## Meshe składające się z pojedynczego punktu ##

### Źródła światła ###

_Wszystkie meshe opisane w tym podpunkcie są multiplikowane (ich ilość wypadało by odczytać z zewnętrznego pliku. Nazwy kolejnych meshy są tworzone przez zmianę jednej cyfry na końcu nazwy mesha, początkowo tą cyfrą jest 0. Przykładowo dla pozycji **GlupiPunkt** mamy: **GlupiPunkt0, GlupiPunkt1**_

  * LightSourceHeadLight - _białe światło mijania przednie_
  * LightSourceRearRed - _czerwone światło mijania tylne_
  * LightSourceRearStop - _czerwone światło stop tylne_
  * LightSourceRearWhite - _białe światło cofania tylne_

### Punkty styku z podłożem ###

_Takie punkty są zawsze cztery, w przypadku większej ilości kół znajdują się one pod środkiem najbardziej zewnętrznych opon. (Ich zastosowanie przewidziane jest do sprawdzania czy koła znajdują się nad czy pod powierzchnią - oczywiście jeżeli są pod to wiemy, że model został niepoprawnie obrócony na wybojach)._

_Pojazdy poziomu 3 nie posiadają poniższych punktów - z założenia pojazdy poziomu 3 są pojazdami lewitującymi / latającymi / może strzele jakiś transportowy poduszkowiec - stąd brak kół i brak punktów styku z powierzchnią._

  * LeftFrontWheelCP
  * LeftRearWheelCP
  * RightFrontWheelCP
  * RightRearWheelCP

## Meshe opisujące obiekt ##

Mesh zawierający główny szkielet pojazdu nosi nazwę **BodyWrok**

### Koła ###

_Meshe kół są multiplikowane, nazewnictwo kolejnych zgodnie z wcześniej opisanym standardem._

_Ilość kół należy odczytać z zewnątrz, ponieważ możemy mieć różne ilości kół - np. 2 osie skrętne, mesh opisujący całą oś kół, czy 8 osobnych opon. W przypadku dużego dźwigu możemy się spodziewać dwóch osi kół przednich._

_Przyjęto założenie, że pojazdy nie będą posiadały osi przeciwnie skrętnych (przykład: przy skręcie w lewo koła przedniej osi skręcają w lewo, tylnej w prawo) - zakładamy że tylna oś nie ma możliwości skrętu_

  * FrontWheel - _koło przednie_
  * RearWheel - _koło tylne_

### Szyby ###

_Aktualnie wszystkie statyczne szyby (szyby przednie, boczne, tylne) mieszczą się w ramach jednego mesha_ Window0.


_W przypadku zmian w tym temacie zostanie umieszczona tutaj informacja, a pozostałe meshe zaktualizowane._

### Drzwi ###

_Meshe są multiplikowane (ponieważ jedne drzwi mogą się składać z wielu materiałów)._

_Nazewnictwo zgodnie z wcześniej przyjętym standardem, grupowane w grupy o nazwie bez sufiksu liczbowego - aczkolwiek nie wiem czy jesteście w stanie to odczytać). Punkty środkowe meshy są obrane w tym samym miejscu._

  * DoorLeft - _drzwi lewe_
  * DoorRight - _drzwi prawe_

### Nadwozie ###

_Aktualnie wszystkie części nadwozia mieszczą się w jednym teksturowanym meshu_ BodyWork.

_W przypadku zmian w tym temacie zostanie umieszczona tutaj informacja, a pozostałe meshe zaktualizowane._

# Meshe specyficzne dla pojazdu #

## Firetruck 2 ##

### Meshe składające się z pojedynczego punktu ###

_W przypadku gdy meshe są multiplikowane zgodnie z wcześniej ustalonym standardem będzie to sygnalizowane znakiem % w miejscu sufiksu liczbowego._

  * LightSourceAlertBlue - _niebieskie migające światło koguta_
  * LightSourceAlertRed - _czerwone migające światło koguta_
  * LightSourceRearBlue% - _niebieskie migające światła tylne ostrzegające (sztuk 2)_
  * WaterSource% - _źródło efektu cząsteczkowego - woda do gaszenia pożaru (sztuk 2)_

### Meshe opisujące obiekt ###

_Poniższe meshe posiadają swój "środek" w tym samym wspólnym dla trzech miejscu._

_W przypadku gdy meshe są multiplikowane zgodnie z wcześniej ustalonym standardem będzie to sygnalizowane znakiem % w miejscu sufiksu liczbowego._

  * WaterCannonBase - _podstawa działka wodnego_
  * WaterPipe% - _dysza działka wodnego (sztuk 2)_

# Uwaga do nazewnictwa modeli #

Wielokrotnie w kodzie zauważałem zmianę nazwy modelu firetruck2 na firetruck. Mimo, że tłumaczyłem to kilka razy jakoś zapominacie o tym, że firetruck2 to wóz strażacki poziomu 2, co oznacza że oprócz niego w grze będzie również firetruck1 jak i firetruck 3 (kolejno wcześniejsza i późniejsza ewolucja). Z tego powodu aktualnie powstający model podwozia dla samochodów wsparcia technicznego pierwszego poziomu nazywa się chassy1 (gdyż jest pierwszego poziomu). Co implikuje, że w tym wypadku sufiksy liczbowe są ważne i nie powinno się ich omijać.