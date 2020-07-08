# Splunk-ETW

A Splunk Technology Add-on to forward filtered ETW events.

The main purpose of this plugin is to select, filter and forward ETW events to Splunk.

## Build from source

`Splunk-ETW` is written in C# and powered by cmake:

```
git clone https://github.com/airbus-cert/Splunk-ETW
mkdir build
cd build
cmake ..\Splunk-ETW
cmake --build . --target package --config release
```

These commands will produce `Splunk-ETW.tar.gz`.

To build the Test solution:

```
cmake ..\Splunk-ETW -DBUILD_TESTS=ON
```

## Install add-on

Download the `Splunk-ETW.tar.gz` from the [latest release](https://github.com/airbus-cert/Splunk-ETW/releases/latest).
Then you can simply install the add-on using the `splunk.exe` command-line tool:

```
splunk.exe install app .\Splunk-ETW.tar.gz
splunk.exe enable app Splunk-ETW
```

Then you have to add the `Splunk-ETW` stanza to your main `inputs.conf`. The value must match an entry in the `profile/` folder. By default, there is a single `cert` profile provided. The associated `inputs.conf` stanza would look like that:

```
[Splunk-ETW://cert]
```

Then just restart the Splunk service:
```
splunk.exe restart
```

You can of course add your own profiles!

## Creating a profile

`Splunk-ETW` can load multiple profiles from the `profile` folder.
Each profile is an INI file describing which events will be forwarded to the Splunk indexer.

To add a `foo` profile:

* Create a `foo.ini` file inside the `profile` folder.
* Add the associated line into the main `CMakeList.txt` file (copy and adjust the line under`Install config files`).

To use the `foo` profile, just add the following line into the `inputs.conf` of the Splunk Universal Forwarder:

```
[Splunk-ETW://foo]
```

## Configuring the profile

Now that you have your profile registered into the Forwarder, edit the `.ini` file and add the providers and filters you want (see details below).

Once you are satisfied with your profile, rebuild and reinstall the project as previously described. 

### Adding a provider by name

To add a provider *by name* just add the following line into your profile file:
```
[Microsoft-Windows-WMI-Activity]
```

To add a provider by GUID you have to specify the type of provider:
* *Manifest* for manifest-based provider
* *TL* TraceLogging provider
* *WPP* Windows PreProcessor provider

```
; identical to [Microsoft-Windows-WMI-Activity]
[Manifest{1418EF04-B0B4-4623-BF7E-D74AB47BBDAA}]

; TraceLogging provider
[TL{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}]

; Windows Pre Processor provider
[WPP{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}]
```

### Filtering by ID

In order to forward only some events produced by a provider, you can specify the relevant event ID:
```
; only forward event id 23 of Microsoft-Windows-WMI-Activity provider
[Microsoft-Windows-WMI-Activity://23]
```

You can add several IDs by adding more stanzas:

```
; Forward events 22 and 23 of Microsoft-Windows-WMI-Activity provider

[Microsoft-Windows-WMI-Activity://22]

[Microsoft-Windows-WMI-Activity://23]
```

You can filter by event ID for all kinds of providers:
```
; Filter event ID 23 of Manifest provider 1418EF04-B0B4-4623-BF7E-D74AB47BBDAA
[Manifest{1418EF04-B0B4-4623-BF7E-D74AB47BBDAA}://23]
```

### Filtering by field value

`Splunk-ETW` can filter forwarded events by field value. We can add a list of `<field_nalme> = <field_value>` under the event forward definition.
All events which have the field with the associated value will be forwarded. For example, event ID `23` of the `Microsoft-Windows-WMI-Activity` provider has a field named `IsLocal`.
This field takes the value `0` when a process is created using the `Win32_Process::Create` method through the network, like in a lateral movement scenario.
If we are only interested in this scenario we can apply the following filter:

```
[Microsoft-Windows-WMI-Activity://23]
IsLocal = 0
```

## Airbus CERT Profile

The Airbus CERT default profile comes with the following simple use-cases:

**Detecting remote execution via WMI**

WMI allows creating processes through the `WIN32_Process` class which exposes a static `Create` method. It can be invoked using the folowing PowerShell command:

```
Invoke-WmiMethod -Class Win32_Process -Name Create -ArgumentList "cmd /c systeminfo"
```

This method can be used for lateral movement.

WMI has a dedicated ETW provider named `Microsoft-Windows-WMI-Activity` and a dedicated event ID for the `Create` method: `23`. `Splunk-ETW` will forward these events with the following config line:

```
[Microsoft-Windows-WMI-Activity://23]
```

**Detecting PrintDemon**

PrintDemon, aka *CVE-2020-1048*, can be detected by monitoring printer driver installation; in particular the installation of the driver named `Generic / Text Only`.
The ETW provider `Microsoft-Windows-PrintService` generates event ID `316` with the name of the driver included into the `Param1` field.

`Splunk-ETW` can forward this event if and only if the value of `Param1` is equal to `Generic / Text Only`, with the following config line:
```
[Microsoft-Windows-PrintService://316]
Param1=Generic / Text Only
```

**Detecting BlueKeep**

BlueKeep, aka *CVE-2019-0708*, is a vulnerability which targets the RDP protocol. Exploiting this vulnerability requires closing a particular channel named `ms_t120`.
The ETW provider `Microsoft-Windows-RemoteDesktopServices-RdpCoreTS` monitors all closed channels by firing event ID `148` with the field `ChannelName` set to the name of the channel.

`Splunk-ETW` can forward this event if and only if the value of `ChannelName` is equal to `ms_t120`, with the following config line:

```
[Microsoft-Windows-RemoteDesktopServices-RdpCoreTS://148]
ChannelName=ms_t120
```

# Credits and references

Greetz to [vector-sec](https://github.com/vector-sec/) for his original work on [TA_ETW](https://github.com/vector-sec/TA_ETW)!

Our previous work on ETW:

* [SSTIC 2020 presentation (in French)](https://sstic.org/2020/presentation/quand_les_bleus_se_prennent_pour_des_chercheurs_de_vulnrabilites/)
* [Winshark](https://github.com/airbus-cert/Winshark): read ETW in WireShark
* [etwbreaker](https://github.com/airbus-cert/etwbreaker): IDA plugin to find ETW providers
