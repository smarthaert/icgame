# Struktura pliku #

Opis jednostek ujęty jest w tagi `<GameObjects></Gameobjects>`.

# Opis jednostki #

Poszczególne jednostki zawarte są w tagach `<GameObject>`, którego parametry to **Name**, który powinien być zgodny z nazwą pliku fbx z modelem, oraz **Type**, który powinien odpowiadać specjalizacji jednostki.

Później następuje lista parametrów, zawartych w tagach `<GameObjectAttribute>`, których parametry **AttributeName** oraz **AttributeType** odpowiadają nazwie i typowi parametrów, zaś wartość - wartości atrybutu.

# Import do ICGame #

Aby odczytać nowy parametr wewnątrz gry należy:
  * Dodać pole w klasie typu `ObjectStats` (w zależności od docelowej widoczności parametru)
  * Dodać odczyt atrybutu w metodzie `GameObjectStatsReader.GetObjectStats(string)`.
W tym momencie parametr jest gotowy do wykorzystania w konstruktorze odpowiedniej klasy z drzewka `GameObject`.

_**TODO:** Atrybuty wymagane dla poszczególnych typów jednostek._

# Wymagane atrybuty #

Każdy obiekt musi implementować atrybuty dla swojej klasy i wszystkich klas, po których dziedziczy.

## GameObject ##

## Unit _: GameObject_ ##

  * `Speed [float]`
  * `TurnRadius [float]`
  * `SubModel [string]` o wartości `selection_ring` - każda jednostka musi mieć możliwość wyświetlenia selection ring'a

## Vehicle _: Unit_ ##

  * `FrontWheelsCount [int]`
  * `RearWheelsCount [int]`
  * `DoorCount [int]` - **ilość par drzwi**
  * `HasTurret [bool]`
  * `WaterSourceCount [int]` - tylko jeśli `HasTurret == true`

## Infantry _: Unit_ ##

## Building _: GameObject_ ##

## StaticObject _: GameObject_ ##

## Civilian _: GameObject_ ##

# Atrybuty opcjonalne #

  * `Effect [string]` - wartość jest nazwą efektu cząsteczkowego przypisywanego do obiektu
  * `SubModel [string]` - wartość jest nazwą modelu składowego. <br>Dodatkowe atrybuty:<br>
<ul><li><code>Position [float float float]</code> - translacja względem pozycji rodzica. Domyślnie <code>0 0 0</code>
</li><li><code>Rotation [float float float]</code> - rotacja względem rodzica. Domyślnie <code>0 0 0</code>
</li><li><code>Scale [float float float]</code> - skalowanie względem rodzica. Domyślnie <code>1 1 1</code>