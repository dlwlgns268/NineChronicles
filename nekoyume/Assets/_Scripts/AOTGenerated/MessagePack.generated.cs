// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

using System.Security.Cryptography;
using Bencodex.Types;
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Resolvers
{
    public class GeneratedResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new GeneratedResolver();

        private GeneratedResolver()
        {
        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.Formatter;
        }

        private static class FormatterCache<T>
        {
            internal static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> Formatter;

            static FormatterCache()
            {
                var f = GeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    Formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedResolverGetFormatterHelper
    {
        private static readonly global::System.Collections.Generic.Dictionary<global::System.Type, int> lookup;

        static GeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<global::System.Type, int>(2)
            {
                { typeof(global::System.Collections.Generic.Dictionary<string, IValue>), 0 },
                { typeof(global::Nekoyume.Action.NCActionEvaluation), 1 },
            };
        }

        internal static object GetFormatter(global::System.Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key))
            {
                return null;
            }

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.DictionaryFormatter<string, IValue>();
                case 1: return new MessagePack.Formatters.Nekoyume.Action.NCActionEvaluationFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1649 // File name should match first type name




// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1129 // Do not use default value type constructor
#pragma warning disable SA1309 // Field names should not begin with underscore
#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Formatters.Nekoyume.Action
{
    public sealed class NCActionEvaluationFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Nekoyume.Action.NCActionEvaluation>
    {
        private readonly global::Lib9c.Formatters.NCActionFormatter __ActionCustomFormatter__ = new global::Lib9c.Formatters.NCActionFormatter();
        private readonly global::Lib9c.Formatters.AddressFormatter __SignerCustomFormatter__ = new global::Lib9c.Formatters.AddressFormatter();
        private readonly global::Lib9c.Formatters.HashDigestFormatter __OutputStateCustomFormatter__ = new global::Lib9c.Formatters.HashDigestFormatter();
        private readonly global::Lib9c.Formatters.ExceptionFormatter<global::System.Exception> __ExceptionCustomFormatter__ = new global::Lib9c.Formatters.ExceptionFormatter<global::System.Exception>();
        private readonly global::Lib9c.Formatters.HashDigestFormatter __PreviousStateCustomFormatter__ = new global::Lib9c.Formatters.HashDigestFormatter();
        private readonly global::Lib9c.Formatters.TxIdFormatter __TxIdCustomFormatter__ = new global::Lib9c.Formatters.TxIdFormatter();

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Nekoyume.Action.NCActionEvaluation value, global::MessagePack.MessagePackSerializerOptions options)
        {
            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(9);
            this.__ActionCustomFormatter__.Serialize(ref writer, value.Action, options);
            this.__SignerCustomFormatter__.Serialize(ref writer, value.Signer, options);
            writer.Write(value.BlockIndex);
            this.__OutputStateCustomFormatter__.Serialize(ref writer, value.OutputState, options);
            this.__ExceptionCustomFormatter__.Serialize(ref writer, value.Exception, options);
            this.__PreviousStateCustomFormatter__.Serialize(ref writer, value.PreviousState, options);
            writer.Write(value.RandomSeed);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, IValue>>(formatterResolver).Serialize(ref writer, value.Extra, options);
            this.__TxIdCustomFormatter__.Serialize(ref writer, value.TxId, options);
        }

        public global::Nekoyume.Action.NCActionEvaluation Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                throw new global::System.InvalidOperationException("typecode is null, struct not supported");
            }

            options.Security.DepthStep(ref reader);
            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var __Action__ = default(global::Nekoyume.Action.ActionBase);
            var __Signer__ = default(global::Libplanet.Crypto.Address);
            var __BlockIndex__ = default(long);
            var __OutputState__ = default(global::Libplanet.Common.HashDigest<SHA256>);
            var __Exception__ = default(global::System.Exception);
            var __PreviousState__ = default(global::Libplanet.Common.HashDigest<SHA256>);
            var __RandomSeed__ = default(int);
            var __Extra__ = default(global::System.Collections.Generic.Dictionary<string, IValue>);
            var __TxId__ = default(global::Libplanet.Types.Tx.TxId?);

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        __Action__ = this.__ActionCustomFormatter__.Deserialize(ref reader, options);
                        break;
                    case 1:
                        __Signer__ = this.__SignerCustomFormatter__.Deserialize(ref reader, options);
                        break;
                    case 2:
                        __BlockIndex__ = reader.ReadInt64();
                        break;
                    case 3:
                        __OutputState__ = this.__OutputStateCustomFormatter__.Deserialize(ref reader, options);
                        break;
                    case 4:
                        __Exception__ = this.__ExceptionCustomFormatter__.Deserialize(ref reader, options);
                        break;
                    case 5:
                        __PreviousState__ = this.__PreviousStateCustomFormatter__.Deserialize(ref reader, options);
                        break;
                    case 6:
                        __RandomSeed__ = reader.ReadInt32();
                        break;
                    case 7:
                        __Extra__ = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, IValue>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 8:
                        __TxId__ = this.__TxIdCustomFormatter__.Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            var ____result = new global::Nekoyume.Action.NCActionEvaluation(__Action__, __Signer__, __BlockIndex__, __OutputState__, __Exception__, __PreviousState__, __RandomSeed__, __Extra__, __TxId__);
            reader.Depth--;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1129 // Do not use default value type constructor
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1649 // File name should match first type name

