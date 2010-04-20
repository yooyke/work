using System;
using System.Collections.Generic;
using OpenMetaverse;

namespace OxViewer.LibOMV
{
    public class PrimitiveBaseShape
    {
        private byte[] m_textureEntry;

        private ushort _pathBegin;
        private byte _pathCurve;
        private ushort _pathEnd;
        private sbyte _pathRadiusOffset;
        private byte _pathRevolutions;
        private byte _pathScaleX;
        private byte _pathScaleY;
        private byte _pathShearX;
        private byte _pathShearY;
        private sbyte _pathSkew;
        private sbyte _pathTaperX;
        private sbyte _pathTaperY;
        private sbyte _pathTwist;
        private sbyte _pathTwistBegin;
        private byte _pCode;
        private ushort _profileBegin;
        private ushort _profileEnd;
        private ushort _profileHollow;
        private Vector3 _scale;
        private byte _state;

        // Sculpted
        private UUID _sculptTexture = UUID.Zero;
        private byte _sculptType = (byte)0;
        private byte[] _sculptData = new byte[0];

        // Flexi
        private int _flexiSoftness = 0;
        private float _flexiTension = 0f;
        private float _flexiDrag = 0f;
        private float _flexiGravity = 0f;
        private float _flexiWind = 0f;
        private float _flexiForceX = 0f;
        private float _flexiForceY = 0f;
        private float _flexiForceZ = 0f;

        //Bright n sparkly
        private float _lightColorR = 0f;
        private float _lightColorG = 0f;
        private float _lightColorB = 0f;
        private float _lightColorA = 1f;
        private float _lightRadius = 0f;
        private float _lightCutoff = 0f;
        private float _lightFalloff = 0f;
        private float _lightIntensity = 1f;
        private bool _flexiEntry = false;
        private bool _lightEntry = false;
        private bool _sculptEntry = false;

        public PrimitiveBaseShape(Primitive prim)
        {
            ExtraParams = new byte[1];

            _pathBegin = Primitive.PackBeginCut(prim.PrimData.PathBegin);
            _pathCurve = (byte)prim.PrimData.PathCurve;
            _pathEnd = Primitive.PackEndCut(prim.PrimData.PathEnd);
            _pathRadiusOffset = Primitive.PackPathTwist(prim.PrimData.PathRadiusOffset);
            _pathRevolutions = Primitive.PackPathRevolutions(prim.PrimData.PathRevolutions);
            _pathScaleX = Primitive.PackPathScale(prim.PrimData.PathScaleX);
            _pathScaleY = Primitive.PackPathScale(prim.PrimData.PathScaleY);
            _pathShearX = (byte)Primitive.PackPathShear(prim.PrimData.PathShearX);
            _pathShearY = (byte)Primitive.PackPathShear(prim.PrimData.PathShearY);
            _pathSkew = Primitive.PackPathTwist(prim.PrimData.PathSkew);
            _pathTaperX = Primitive.PackPathTaper(prim.PrimData.PathTaperX);
            _pathTaperY = Primitive.PackPathTaper(prim.PrimData.PathTaperY);
            _pathTwist = Primitive.PackPathTwist(prim.PrimData.PathTwist);
            _pathTwistBegin = Primitive.PackPathTwist(prim.PrimData.PathTwistBegin);
            _pCode = (byte)prim.PrimData.PCode;
            _profileBegin = Primitive.PackBeginCut(prim.PrimData.ProfileBegin);
            _profileEnd = Primitive.PackEndCut(prim.PrimData.ProfileEnd);
            _profileHollow = Primitive.PackProfileHollow(prim.PrimData.ProfileHollow);
            _scale = prim.Scale;
            _state = prim.PrimData.State;
        }

        public Primitive.TextureEntry Textures
        {
            get
            {
                return new Primitive.TextureEntry(m_textureEntry, 0, m_textureEntry.Length);
            }

            set { m_textureEntry = value.ToBytes(); }
        }

        public byte[] TextureEntry
        {
            get { return m_textureEntry; }

            set
            {
                if (value == null)
                    m_textureEntry = new byte[1];
                else
                    m_textureEntry = value;
            }
        }

        public void SetScale(float side)
        {
            _scale = new Vector3(side, side, side);
        }

        public void SetHeigth(float heigth)
        {
            _scale.Z = heigth;
        }

        public void SetRadius(float radius)
        {
            _scale.X = _scale.Y = radius * 2f;
        }

        // TODO: void returns need to change of course
        public virtual void GetMesh()
        {
        }

        public PrimitiveBaseShape Copy()
        {
            return (PrimitiveBaseShape)MemberwiseClone();
        }

        public void SetPathRange(Vector3 pathRange)
        {
            _pathBegin = Primitive.PackBeginCut(pathRange.X);
            _pathEnd = Primitive.PackEndCut(pathRange.Y);
        }

        public void SetSculptData(byte sculptType, UUID SculptTextureUUID)
        {
            _sculptType = sculptType;
            _sculptTexture = SculptTextureUUID;
        }

        public void SetProfileRange(Vector3 profileRange)
        {
            _profileBegin = Primitive.PackBeginCut(profileRange.X);
            _profileEnd = Primitive.PackEndCut(profileRange.Y);
        }

        public byte[] ExtraParams
        {
            get
            {
                return ExtraParamsToBytes();
            }
            set
            {
                ReadInExtraParamsBytes(value);
            }
        }

        public ushort PathBegin
        {
            get
            {
                return _pathBegin;
            }
            set
            {
                _pathBegin = value;
            }
        }

        public byte PathCurve
        {
            get
            {
                return _pathCurve;
            }
            set
            {
                _pathCurve = value;
            }
        }

        public ushort PathEnd
        {
            get
            {
                return _pathEnd;
            }
            set
            {
                _pathEnd = value;
            }
        }

        public sbyte PathRadiusOffset
        {
            get
            {
                return _pathRadiusOffset;
            }
            set
            {
                _pathRadiusOffset = value;
            }
        }

        public byte PathRevolutions
        {
            get
            {
                return _pathRevolutions;
            }
            set
            {
                _pathRevolutions = value;
            }
        }

        public byte PathScaleX
        {
            get
            {
                return _pathScaleX;
            }
            set
            {
                _pathScaleX = value;
            }
        }

        public byte PathScaleY
        {
            get
            {
                return _pathScaleY;
            }
            set
            {
                _pathScaleY = value;
            }
        }

        public byte PathShearX
        {
            get
            {
                return _pathShearX;
            }
            set
            {
                _pathShearX = value;
            }
        }

        public byte PathShearY
        {
            get
            {
                return _pathShearY;
            }
            set
            {
                _pathShearY = value;
            }
        }

        public sbyte PathSkew
        {
            get
            {
                return _pathSkew;
            }
            set
            {
                _pathSkew = value;
            }
        }

        public sbyte PathTaperX
        {
            get
            {
                return _pathTaperX;
            }
            set
            {
                _pathTaperX = value;
            }
        }

        public sbyte PathTaperY
        {
            get
            {
                return _pathTaperY;
            }
            set
            {
                _pathTaperY = value;
            }
        }

        public sbyte PathTwist
        {
            get
            {
                return _pathTwist;
            }
            set
            {
                _pathTwist = value;
            }
        }

        public sbyte PathTwistBegin
        {
            get
            {
                return _pathTwistBegin;
            }
            set
            {
                _pathTwistBegin = value;
            }
        }

        public byte PCode
        {
            get
            {
                return _pCode;
            }
            set
            {
                _pCode = value;
            }
        }

        public ushort ProfileBegin
        {
            get
            {
                return _profileBegin;
            }
            set
            {
                _profileBegin = value;
            }
        }

        public ushort ProfileEnd
        {
            get
            {
                return _profileEnd;
            }
            set
            {
                _profileEnd = value;
            }
        }

        public ushort ProfileHollow
        {
            get
            {
                return _profileHollow;
            }
            set
            {
                _profileHollow = value;
            }
        }

        public Vector3 Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
            }
        }

        public byte State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public UUID SculptTexture
        {
            get
            {
                return _sculptTexture;
            }
            set
            {
                _sculptTexture = value;
            }
        }

        public byte SculptType
        {
            get
            {
                return _sculptType;
            }
            set
            {
                _sculptType = value;
            }
        }

        public byte[] SculptData
        {
            get
            {
                return _sculptData;
            }
            set
            {
                _sculptData = value;
            }
        }

        public int FlexiSoftness
        {
            get
            {
                return _flexiSoftness;
            }
            set
            {
                _flexiSoftness = value;
            }
        }

        public float FlexiTension
        {
            get
            {
                return _flexiTension;
            }
            set
            {
                _flexiTension = value;
            }
        }

        public float FlexiDrag
        {
            get
            {
                return _flexiDrag;
            }
            set
            {
                _flexiDrag = value;
            }
        }

        public float FlexiGravity
        {
            get
            {
                return _flexiGravity;
            }
            set
            {
                _flexiGravity = value;
            }
        }

        public float FlexiWind
        {
            get
            {
                return _flexiWind;
            }
            set
            {
                _flexiWind = value;
            }
        }

        public float FlexiForceX
        {
            get
            {
                return _flexiForceX;
            }
            set
            {
                _flexiForceX = value;
            }
        }

        public float FlexiForceY
        {
            get
            {
                return _flexiForceY;
            }
            set
            {
                _flexiForceY = value;
            }
        }

        public float FlexiForceZ
        {
            get
            {
                return _flexiForceZ;
            }
            set
            {
                _flexiForceZ = value;
            }
        }

        public float LightColorR
        {
            get
            {
                return _lightColorR;
            }
            set
            {
                _lightColorR = value;
            }
        }

        public float LightColorG
        {
            get
            {
                return _lightColorG;
            }
            set
            {
                _lightColorG = value;
            }
        }

        public float LightColorB
        {
            get
            {
                return _lightColorB;
            }
            set
            {
                _lightColorB = value;
            }
        }

        public float LightColorA
        {
            get
            {
                return _lightColorA;
            }
            set
            {
                _lightColorA = value;
            }
        }

        public float LightRadius
        {
            get
            {
                return _lightRadius;
            }
            set
            {
                _lightRadius = value;
            }
        }

        public float LightCutoff
        {
            get
            {
                return _lightCutoff;
            }
            set
            {
                _lightCutoff = value;
            }
        }

        public float LightFalloff
        {
            get
            {
                return _lightFalloff;
            }
            set
            {
                _lightFalloff = value;
            }
        }

        public float LightIntensity
        {
            get
            {
                return _lightIntensity;
            }
            set
            {
                _lightIntensity = value;
            }
        }

        public bool FlexiEntry
        {
            get
            {
                return _flexiEntry;
            }
            set
            {
                _flexiEntry = value;
            }
        }

        public bool LightEntry
        {
            get
            {
                return _lightEntry;
            }
            set
            {
                _lightEntry = value;
            }
        }

        public bool SculptEntry
        {
            get
            {
                return _sculptEntry;
            }
            set
            {
                _sculptEntry = value;
            }
        }

        public byte[] ExtraParamsToBytes()
        {
            ushort FlexiEP = 0x10;
            ushort LightEP = 0x20;
            ushort SculptEP = 0x30;

            int i = 0;
            uint TotalBytesLength = 1; // ExtraParamsNum

            uint ExtraParamsNum = 0;
            if (_flexiEntry)
            {
                ExtraParamsNum++;
                TotalBytesLength += 16;// data
                TotalBytesLength += 2 + 4; // type
            }
            if (_lightEntry)
            {
                ExtraParamsNum++;
                TotalBytesLength += 16;// data
                TotalBytesLength += 2 + 4; // type
            }
            if (_sculptEntry)
            {
                ExtraParamsNum++;
                TotalBytesLength += 17;// data
                TotalBytesLength += 2 + 4; // type
            }

            byte[] returnbytes = new byte[TotalBytesLength];


            // uint paramlength = ExtraParamsNum;

            // Stick in the number of parameters
            returnbytes[i++] = (byte)ExtraParamsNum;

            if (_flexiEntry)
            {
                byte[] FlexiData = GetFlexiBytes();

                returnbytes[i++] = (byte)(FlexiEP % 256);
                returnbytes[i++] = (byte)((FlexiEP >> 8) % 256);

                returnbytes[i++] = (byte)(FlexiData.Length % 256);
                returnbytes[i++] = (byte)((FlexiData.Length >> 8) % 256);
                returnbytes[i++] = (byte)((FlexiData.Length >> 16) % 256);
                returnbytes[i++] = (byte)((FlexiData.Length >> 24) % 256);
                Array.Copy(FlexiData, 0, returnbytes, i, FlexiData.Length);
                i += FlexiData.Length;
            }
            if (_lightEntry)
            {
                byte[] LightData = GetLightBytes();

                returnbytes[i++] = (byte)(LightEP % 256);
                returnbytes[i++] = (byte)((LightEP >> 8) % 256);

                returnbytes[i++] = (byte)(LightData.Length % 256);
                returnbytes[i++] = (byte)((LightData.Length >> 8) % 256);
                returnbytes[i++] = (byte)((LightData.Length >> 16) % 256);
                returnbytes[i++] = (byte)((LightData.Length >> 24) % 256);
                Array.Copy(LightData, 0, returnbytes, i, LightData.Length);
                i += LightData.Length;
            }
            if (_sculptEntry)
            {
                byte[] SculptData = GetSculptBytes();

                returnbytes[i++] = (byte)(SculptEP % 256);
                returnbytes[i++] = (byte)((SculptEP >> 8) % 256);

                returnbytes[i++] = (byte)(SculptData.Length % 256);
                returnbytes[i++] = (byte)((SculptData.Length >> 8) % 256);
                returnbytes[i++] = (byte)((SculptData.Length >> 16) % 256);
                returnbytes[i++] = (byte)((SculptData.Length >> 24) % 256);
                Array.Copy(SculptData, 0, returnbytes, i, SculptData.Length);
                i += SculptData.Length;
            }

            if (!_flexiEntry && !_lightEntry && !_sculptEntry)
            {
                byte[] returnbyte = new byte[1];
                returnbyte[0] = 0;
                return returnbyte;
            }


            return returnbytes;
        }

        public void ReadInUpdateExtraParam(ushort type, bool inUse, byte[] data)
        {
            const ushort FlexiEP = 0x10;
            const ushort LightEP = 0x20;
            const ushort SculptEP = 0x30;

            switch (type)
            {
                case FlexiEP:
                    if (!inUse)
                    {
                        _flexiEntry = false;
                        return;
                    }
                    ReadFlexiData(data, 0);
                    break;

                case LightEP:
                    if (!inUse)
                    {
                        _lightEntry = false;
                        return;
                    }
                    ReadLightData(data, 0);
                    break;

                case SculptEP:
                    if (!inUse)
                    {
                        _sculptEntry = false;
                        return;
                    }
                    ReadSculptData(data, 0);
                    break;
            }
        }

        public void ReadInExtraParamsBytes(byte[] data)
        {
            if (data == null)
                return;

            const ushort FlexiEP = 0x10;
            const ushort LightEP = 0x20;
            const ushort SculptEP = 0x30;

            bool lGotFlexi = false;
            bool lGotLight = false;
            bool lGotSculpt = false;

            int i = 0;
            byte extraParamCount = 0;
            if (data.Length > 0)
            {
                extraParamCount = data[i++];
            }


            for (int k = 0; k < extraParamCount; k++)
            {
                ushort epType = Utils.BytesToUInt16(data, i);

                i += 2;
                // uint paramLength = Helpers.BytesToUIntBig(data, i);

                i += 4;
                switch (epType)
                {
                    case FlexiEP:
                        ReadFlexiData(data, i);
                        i += 16;
                        lGotFlexi = true;
                        break;

                    case LightEP:
                        ReadLightData(data, i);
                        i += 16;
                        lGotLight = true;
                        break;

                    case SculptEP:
                        ReadSculptData(data, i);
                        i += 17;
                        lGotSculpt = true;
                        break;
                }
            }

            if (!lGotFlexi)
                _flexiEntry = false;
            if (!lGotLight)
                _lightEntry = false;
            if (!lGotSculpt)
                _sculptEntry = false;

        }

        public void ReadSculptData(byte[] data, int pos)
        {
            byte[] SculptTextureUUID = new byte[16];
            UUID SculptUUID = UUID.Zero;
            byte SculptTypel = data[16 + pos];

            if (data.Length + pos >= 17)
            {
                _sculptEntry = true;
                SculptTextureUUID = new byte[16];
                SculptTypel = data[16 + pos];
                Array.Copy(data, pos, SculptTextureUUID, 0, 16);
                SculptUUID = new UUID(SculptTextureUUID, 0);
            }
            else
            {
                _sculptEntry = false;
                SculptUUID = UUID.Zero;
                SculptTypel = 0x00;
            }

            if (_sculptEntry)
            {
                if (_sculptType != (byte)1 && _sculptType != (byte)2 && _sculptType != (byte)3 && _sculptType != (byte)4)
                    _sculptType = 4;
            }
            _sculptTexture = SculptUUID;
            _sculptType = SculptTypel;
            //m_log.Info("[SCULPT]:" + SculptUUID.ToString());
        }

        public byte[] GetSculptBytes()
        {
            byte[] data = new byte[17];

            _sculptTexture.GetBytes().CopyTo(data, 0);
            data[16] = (byte)_sculptType;

            return data;
        }

        public void ReadFlexiData(byte[] data, int pos)
        {
            if (data.Length - pos >= 16)
            {
                _flexiEntry = true;
                _flexiSoftness = ((data[pos] & 0x80) >> 6) | ((data[pos + 1] & 0x80) >> 7);

                _flexiTension = (float)(data[pos++] & 0x7F) / 10.0f;
                _flexiDrag = (float)(data[pos++] & 0x7F) / 10.0f;
                _flexiGravity = (float)(data[pos++] / 10.0f) - 10.0f;
                _flexiWind = (float)data[pos++] / 10.0f;
                Vector3 lForce = new Vector3(data, pos);
                _flexiForceX = lForce.X;
                _flexiForceY = lForce.Y;
                _flexiForceZ = lForce.Z;
            }
            else
            {
                _flexiEntry = false;
                _flexiSoftness = 0;

                _flexiTension = 0.0f;
                _flexiDrag = 0.0f;
                _flexiGravity = 0.0f;
                _flexiWind = 0.0f;
                _flexiForceX = 0f;
                _flexiForceY = 0f;
                _flexiForceZ = 0f;
            }
        }

        public byte[] GetFlexiBytes()
        {
            byte[] data = new byte[16];
            int i = 0;

            // Softness is packed in the upper bits of tension and drag
            data[i] = (byte)((_flexiSoftness & 2) << 6);
            data[i + 1] = (byte)((_flexiSoftness & 1) << 7);

            data[i++] |= (byte)((byte)(_flexiTension * 10.01f) & 0x7F);
            data[i++] |= (byte)((byte)(_flexiDrag * 10.01f) & 0x7F);
            data[i++] = (byte)((_flexiGravity + 10.0f) * 10.01f);
            data[i++] = (byte)(_flexiWind * 10.01f);
            Vector3 lForce = new Vector3(_flexiForceX, _flexiForceY, _flexiForceZ);
            lForce.GetBytes().CopyTo(data, i);

            return data;
        }

        public void ReadLightData(byte[] data, int pos)
        {
            if (data.Length - pos >= 16)
            {
                _lightEntry = true;
                Color4 lColor = new Color4(data, pos, false);
                _lightIntensity = lColor.A;
                _lightColorA = 1f;
                _lightColorR = lColor.R;
                _lightColorG = lColor.G;
                _lightColorB = lColor.B;

                _lightRadius = Utils.BytesToFloat(data, pos + 4);
                _lightCutoff = Utils.BytesToFloat(data, pos + 8);
                _lightFalloff = Utils.BytesToFloat(data, pos + 12);
            }
            else
            {
                _lightEntry = false;
                _lightColorA = 1f;
                _lightColorR = 0f;
                _lightColorG = 0f;
                _lightColorB = 0f;
                _lightRadius = 0f;
                _lightCutoff = 0f;
                _lightFalloff = 0f;
                _lightIntensity = 0f;
            }
        }

        public byte[] GetLightBytes()
        {
            byte[] data = new byte[16];

            // Alpha channel in color is intensity
            Color4 tmpColor = new Color4(_lightColorR, _lightColorG, _lightColorB, _lightIntensity);

            tmpColor.GetBytes().CopyTo(data, 0);
            Utils.FloatToBytes(_lightRadius).CopyTo(data, 4);
            Utils.FloatToBytes(_lightCutoff).CopyTo(data, 8);
            Utils.FloatToBytes(_lightFalloff).CopyTo(data, 12);

            return data;
        }
    }
}
