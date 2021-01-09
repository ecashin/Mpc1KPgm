// Thanks to https://mybunnyhug.org/fileformats/pgm/ for the format description.
open System.IO

type PgmHeader = {
    FileSize: uint16
    Padding1: uint16
    FileType: string
    Padding2: uint32
}

// byte-count sizes
let FileTypeSize = 16
let SampleNameSize = 16

type SampleData = {
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

let sampleData (rd: BinaryReader) =
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

type Pad = {
    Sample0: SampleData
    Sample1: SampleData
    Sample2: SampleData
    Sample3: SampleData
    Padding1: uint16
    VoiceOverlap: byte
    MuteGroup: byte
    Padding2: byte
    Unknown: byte
    Attack: byte
    Decay: byte
    DecayMode: byte
    Padding3: uint16
    VelocityToLevel: byte
    Padding4a: uint32
    Padding4b: byte
    Filter1Type: byte
    Filter1Freq: byte
    Filter1Res: byte
    Padding5: uint32
    Filter1VeloToFreq: byte
    Filter2Type: byte
    Filter2Frequency: byte
    Filter2Res: byte
    Padding6: uint32
    Filter2VeloToFreq: byte
    Padding7a: uint32
    Padding7b: uint32
    Padding7c: uint32
    Padding7d: uint16
    MixerLevel: byte
    MixerPan: byte
    Output: byte
    FXSend: byte
    FXSendLevel: byte
    FilterAttenuation: byte
    Padding8a: uint64
    Padding8b: uint32
    Padding8c: uint16
    Padding8d: byte
}

let readPad (rd: BinaryReader) (_: int) =
    {
        Sample0 = (sampleData rd)
        Sample1 = (sampleData rd)
        Sample2 = (sampleData rd)
        Sample3 = (sampleData rd)
        Padding1 = rd.ReadUInt16()
        VoiceOverlap = rd.ReadByte()
        MuteGroup = rd.ReadByte()
        Padding2 = rd.ReadByte()
        Unknown = rd.ReadByte()
        Attack = rd.ReadByte()
        Decay = rd.ReadByte()
        DecayMode = rd.ReadByte()
        Padding3 = rd.ReadUInt16()
        VelocityToLevel = rd.ReadByte()
        Padding4a = rd.ReadUInt32()
        Padding4b = rd.ReadByte()
        Filter1Type = rd.ReadByte()
        Filter1Freq = rd.ReadByte()
        Filter1Res = rd.ReadByte()
        Padding5 = rd.ReadUInt32()
        Filter1VeloToFreq = rd.ReadByte()
        Filter2Type = rd.ReadByte()
        Filter2Frequency = rd.ReadByte()
        Filter2Res = rd.ReadByte()
        Padding6 = rd.ReadUInt32()
        Filter2VeloToFreq = rd.ReadByte()
        Padding7a = rd.ReadUInt32()
        Padding7b = rd.ReadUInt32()
        Padding7c = rd.ReadUInt32()
        Padding7d = rd.ReadUInt16()
        MixerLevel = rd.ReadByte()
        MixerPan = rd.ReadByte()
        Output = rd.ReadByte()
        FXSend = rd.ReadByte()
        FXSendLevel = rd.ReadByte()
        FilterAttenuation = rd.ReadByte()
        Padding8a = rd.ReadUInt64()
        Padding8b = rd.ReadUInt32()
        Padding8c = rd.ReadUInt16()
        Padding8d = rd.ReadByte()
    }

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

let NSliders = 2

[<EntryPoint>]
let main argv =
    match argv with
    | [|pgmFileName|] ->
    use rd = new BinaryReader(File.Open(pgmFileName, FileMode.Open))
    let hdr = {
        FileSize = rd.ReadUInt16()
        Padding1 = rd.ReadUInt16()
        FileType = rd.ReadBytes(FileTypeSize) |> asciiString
        Padding2 = rd.ReadUInt32()
    }
    printfn "%A" hdr
    let pads = Array.init NPads (readPad rd)
    pads |> Array.iteri (fun i x -> printfn "pad%02d: %A" i x)
    let midi = readMidi rd
    printfn "%A" midi
    let sliders = Array.init NSliders (readSlider rd)
    printfn "sliders:\n%A" sliders
    0
    | _ -> 1