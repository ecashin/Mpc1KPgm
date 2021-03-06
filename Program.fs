// Thanks to https://mybunnyhug.org/fileformats/pgm/ for the format description.
open Newtonsoft.Json
open System.IO

type Header = {
    FileSize: uint16
    Padding1: byte []
    FileType: string
    Padding2: byte []
}

let HeaderPadding1Size = 2
let HeaderPadding2Size = 4

// byte-count sizes
let FileTypeSize = 16
let SampleNameSize = 16

type Sample = {
    SampleName: string
    Padding1: byte
    Level: byte
    RangeLower: byte
    RangeUpper: byte
    Tuning: uint16
    PlayMode: byte
    Padding2: byte
}

let asciiString (bytes: byte []) =
    System.Text.Encoding.ASCII.GetString bytes

let asciiBytes (s: string) =
    System.Text.Encoding.ASCII.GetBytes s

let readSample (rd: BinaryReader) =
    {
        SampleName = rd.ReadBytes(SampleNameSize) |> asciiString
        Padding1 = rd.ReadByte()
        Level = rd.ReadByte()
        RangeLower = rd.ReadByte()
        RangeUpper = rd.ReadByte()
        Tuning = rd.ReadUInt16()
        PlayMode = rd.ReadByte()
        Padding2 = rd.ReadByte()
    }

let writeSample (wr: BinaryWriter) (smp: Sample) =
    wr.Write(smp.SampleName |> asciiBytes)
    wr.Write(smp.Padding1)
    wr.Write(smp.Level)
    wr.Write(smp.RangeLower)
    wr.Write(smp.RangeUpper)
    wr.Write(smp.Tuning)
    wr.Write(smp.PlayMode)
    wr.Write(smp.Padding2)

type Pad = {
    Sample0: Sample
    Sample1: Sample
    Sample2: Sample
    Sample3: Sample
    Padding1: byte []
    VoiceOverlap: byte
    MuteGroup: byte
    Padding2: byte
    Unknown: byte
    Attack: byte
    Decay: byte
    DecayMode: byte
    Padding3: byte []
    VelocityToLevel: byte
    Padding4: byte []
    Filter1Type: byte
    Filter1Freq: byte
    Filter1Res: byte
    Padding5: byte []
    Filter1VeloToFreq: byte
    Filter2Type: byte
    Filter2Frequency: byte
    Filter2Res: byte
    Padding6: byte []
    Filter2VeloToFreq: byte
    Padding7: byte []
    MixerLevel: byte
    MixerPan: byte
    Output: byte
    FXSend: byte
    FXSendLevel: byte
    FilterAttenuation: byte
    Padding8: byte []
}

let PadPadding1Size = 2
let PadPadding3Size = 2
let PadPadding4Size = 5
let PadPadding5Size = 4
let PadPadding6Size = 4
let PadPadding7Size = 14
let PadPadding8Size = 15

let readPad (rd: BinaryReader) (_: int) =
    {
        Sample0 = (readSample rd)
        Sample1 = (readSample rd)
        Sample2 = (readSample rd)
        Sample3 = (readSample rd)
        Padding1 = rd.ReadBytes(PadPadding1Size)
        VoiceOverlap = rd.ReadByte()
        MuteGroup = rd.ReadByte()
        Padding2 = rd.ReadByte()
        Unknown = rd.ReadByte()
        Attack = rd.ReadByte()
        Decay = rd.ReadByte()
        DecayMode = rd.ReadByte()
        Padding3 = rd.ReadBytes(PadPadding3Size)
        VelocityToLevel = rd.ReadByte()
        Padding4 = rd.ReadBytes(PadPadding4Size)
        Filter1Type = rd.ReadByte()
        Filter1Freq = rd.ReadByte()
        Filter1Res = rd.ReadByte()
        Padding5 = rd.ReadBytes(PadPadding5Size)
        Filter1VeloToFreq = rd.ReadByte()
        Filter2Type = rd.ReadByte()
        Filter2Frequency = rd.ReadByte()
        Filter2Res = rd.ReadByte()
        Padding6 = rd.ReadBytes(PadPadding6Size)
        Filter2VeloToFreq = rd.ReadByte()
        Padding7 = rd.ReadBytes(PadPadding7Size)
        MixerLevel = rd.ReadByte()
        MixerPan = rd.ReadByte()
        Output = rd.ReadByte()
        FXSend = rd.ReadByte()
        FXSendLevel = rd.ReadByte()
        FilterAttenuation = rd.ReadByte()
        Padding8 = rd.ReadBytes(PadPadding8Size)
    }

let writePad (wr: BinaryWriter) (pad: Pad) =
    writeSample wr pad.Sample0
    writeSample wr pad.Sample1
    writeSample wr pad.Sample2
    writeSample wr pad.Sample3
    wr.Write(pad.Padding1)
    wr.Write(pad.VoiceOverlap)
    wr.Write(pad.MuteGroup)
    wr.Write(pad.Padding2)
    wr.Write(pad.Unknown)
    wr.Write(pad.Attack)
    wr.Write(pad.Decay)
    wr.Write(pad.DecayMode)
    wr.Write(pad.Padding3)
    wr.Write(pad.VelocityToLevel)
    wr.Write(pad.Padding4)
    wr.Write(pad.Filter1Type)
    wr.Write(pad.Filter1Freq)
    wr.Write(pad.Filter1Res)
    wr.Write(pad.Padding5)
    wr.Write(pad.Filter1VeloToFreq)
    wr.Write(pad.Filter2Type)
    wr.Write(pad.Filter2Frequency)
    wr.Write(pad.Filter2Res)
    wr.Write(pad.Padding6)
    wr.Write(pad.Filter2VeloToFreq)
    wr.Write(pad.Padding7)
    wr.Write(pad.MixerLevel)
    wr.Write(pad.MixerPan)
    wr.Write(pad.Output)
    wr.Write(pad.FXSend)
    wr.Write(pad.FXSendLevel)
    wr.Write(pad.FilterAttenuation)
    wr.Write(pad.Padding8)

let NPads = 64

type Midi = {
    PadMidiNoteValues: byte
    MidiNotePadValues: byte
    MidiProgramChange: byte
}

let readMidi (rd: BinaryReader) =
    {
        PadMidiNoteValues = rd.ReadByte()
        MidiNotePadValues = rd.ReadByte()
        MidiProgramChange = rd.ReadByte()
    }

let writeMidi (wr: BinaryWriter) (midi: Midi) =
    wr.Write(midi.PadMidiNoteValues)
    wr.Write(midi.MidiNotePadValues)
    wr.Write(midi.MidiProgramChange)

type Slider = {
    SliderPad: byte
    Unknown: byte
    SliderParameter: byte
    SliderTuneLow: byte
    SliderTuneHigh: byte
    SliderFilterLow: byte
    SliderFilterHigh: byte
    SliderLayerLow: byte
    SliderLayerHigh: byte
    SliderAttackLow: byte
    SliderAttackHigh: byte
    SliderDecayLow: byte
    SliderDecayHigh: byte
}

let readSlider (rd: BinaryReader) (_: int) =
    {
        SliderPad = rd.ReadByte()
        Unknown = rd.ReadByte()
        SliderParameter = rd.ReadByte()
        SliderTuneLow = rd.ReadByte()
        SliderTuneHigh = rd.ReadByte()
        SliderFilterLow = rd.ReadByte()
        SliderFilterHigh = rd.ReadByte()
        SliderLayerLow = rd.ReadByte()
        SliderLayerHigh = rd.ReadByte()
        SliderAttackLow = rd.ReadByte()
        SliderAttackHigh = rd.ReadByte()
        SliderDecayLow = rd.ReadByte()
        SliderDecayHigh = rd.ReadByte()
    }

let writeSlider (wr: BinaryWriter) (slider: Slider) =
    wr.Write(slider.SliderPad)
    wr.Write(slider.Unknown)
    wr.Write(slider.SliderParameter)
    wr.Write(slider.SliderTuneLow)
    wr.Write(slider.SliderTuneHigh)
    wr.Write(slider.SliderFilterLow)
    wr.Write(slider.SliderFilterHigh)
    wr.Write(slider.SliderLayerLow)
    wr.Write(slider.SliderLayerHigh)
    wr.Write(slider.SliderAttackLow)
    wr.Write(slider.SliderAttackHigh)
    wr.Write(slider.SliderDecayLow)
    wr.Write(slider.SliderDecayHigh)

let NSliders = 2

let readHeader (rd: BinaryReader) =
    {
        FileSize = rd.ReadUInt16()
        Padding1 = rd.ReadBytes(HeaderPadding1Size)
        FileType = rd.ReadBytes(FileTypeSize) |> asciiString
        Padding2 = rd.ReadBytes(HeaderPadding2Size)
    }

let writeHeader (wr: BinaryWriter) (hdr: Header) =
    wr.Write(hdr.FileSize)
    wr.Write(hdr.Padding1)
    assert (hdr.FileType.Length = (hdr.FileType |> asciiBytes).Length)
    wr.Write(hdr.FileType |> asciiBytes)
    wr.Write(hdr.Padding2)

type Footer = {
    Padding: byte []
}

let FooterSize = 17

let readFooter (rd: BinaryReader) =
    {
        Padding = rd.ReadBytes(FooterSize)
    }

let writeFooter (wr: BinaryWriter) (footer: Footer) =
    wr.Write(footer.Padding)

type ProgramFile = {
    Header: Header
    Pads: Pad []            // 64
    Midi: Midi
    Sliders: Slider []   // 2
    Footer: Footer
    Extra: byte []
}

let ProgramFileAllFieldsSize = 0x2946

let writePgm (pgm: ProgramFile) (outFileName: string) =
    use wr = new BinaryWriter(File.Open(outFileName, FileMode.Create))
    writeHeader wr pgm.Header
    pgm.Pads |> Array.iter (fun pad -> writePad wr pad)
    writeMidi wr pgm.Midi
    pgm.Sliders |> Array.iter (fun slider -> writeSlider wr slider)
    writeFooter wr pgm.Footer
    wr.Write(pgm.Extra)

let parsePgmFile (pgmFileName: string) =
    use rd = new BinaryReader(File.Open(pgmFileName, FileMode.Open))
    let hdr = (readHeader rd)
    let pads = Array.init NPads (readPad rd)
    let midi = readMidi rd
    let sliders = Array.init NSliders (readSlider rd)
    let footer = (readFooter rd)
    let extraSize = (int hdr.FileSize) - ProgramFileAllFieldsSize
    let extra =
        if extraSize > 0 then
            rd.ReadBytes(extraSize)
        else
            Array.empty<byte>
    {
        Header = hdr
        Pads = pads
        Midi = midi
        Sliders = sliders
        Footer = footer
        Extra = extra
    }

[<EntryPoint>]
let main argv =
    match argv with
    | [|pgmFileName; outPgmFileName|] ->
        let pgm = parsePgmFile pgmFileName
        writePgm pgm outPgmFileName
        0
    | [|pgmFileName|] ->
        parsePgmFile pgmFileName |> JsonConvert.SerializeObject |> printf "%s"
        0
    | _ -> 1