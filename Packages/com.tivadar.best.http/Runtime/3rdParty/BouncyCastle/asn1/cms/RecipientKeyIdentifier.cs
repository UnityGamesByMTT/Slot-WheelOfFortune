#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

using Best.HTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
    public class RecipientKeyIdentifier
        : Asn1Encodable
    {
        private Asn1OctetString      subjectKeyIdentifier;
        private Asn1GeneralizedTime  date;
        private OtherKeyAttribute    other;

		public RecipientKeyIdentifier(
            Asn1OctetString         subjectKeyIdentifier,
            Asn1GeneralizedTime     date,
            OtherKeyAttribute       other)
        {
            this.subjectKeyIdentifier = subjectKeyIdentifier;
            this.date = date;
            this.other = other;
        }
		
		public RecipientKeyIdentifier(
			byte[] subjectKeyIdentifier)
			: this(subjectKeyIdentifier, null, null)
		{
		}

		public RecipientKeyIdentifier(
			byte[]				subjectKeyIdentifier,
            Asn1GeneralizedTime date,
			OtherKeyAttribute	other)
		{
			this.subjectKeyIdentifier = new DerOctetString(subjectKeyIdentifier);
			this.date = date;
			this.other = other;
		}

		public RecipientKeyIdentifier(
            Asn1Sequence seq)
        {
            subjectKeyIdentifier = Asn1OctetString.GetInstance(
				seq[0]);

			switch(seq.Count)
            {
				case 1:
					break;
				case 2:
					if (seq[1] is Asn1GeneralizedTime)
					{
						date = (Asn1GeneralizedTime)seq[1];
					}
					else
					{
						other = OtherKeyAttribute.GetInstance(seq[2]);
					}
					break;
				case 3:
					date  = (Asn1GeneralizedTime)seq[1];
					other = OtherKeyAttribute.GetInstance(seq[2]);
					break;
				default:
					throw new ArgumentException("Invalid RecipientKeyIdentifier");
            }
        }

		/**
         * return a RecipientKeyIdentifier object from a tagged object.
         *
         * @param _ato the tagged object holding the object we want.
         * @param _explicit true if the object is meant to be explicitly
         *              tagged false otherwise.
         * @exception ArgumentException if the object held by the
         *          tagged object cannot be converted.
         */
        public static RecipientKeyIdentifier GetInstance(
			Asn1TaggedObject	ato,
			bool				explicitly)
		{
            return GetInstance(Asn1Sequence.GetInstance(ato, explicitly));
        }

		/**
         * return a RecipientKeyIdentifier object from the given object.
         *
         * @param _obj the object we want converted.
         * @exception ArgumentException if the object cannot be converted.
         */
        public static RecipientKeyIdentifier GetInstance(
			object obj)
		{
            if (obj == null || obj is RecipientKeyIdentifier)
                return (RecipientKeyIdentifier) obj;

			if (obj is Asn1Sequence)
				return new RecipientKeyIdentifier((Asn1Sequence) obj);

            throw new ArgumentException("Invalid RecipientKeyIdentifier: " + Org.BouncyCastle.Utilities.Platform.GetTypeName(obj));
        }

		public Asn1OctetString SubjectKeyIdentifier
		{
			get { return subjectKeyIdentifier; }
		}

		public Asn1GeneralizedTime Date
		{
			get { return date; }
		}

		public OtherKeyAttribute OtherKeyAttribute
		{
			get { return other; }
		}

		/**
         * Produce an object suitable for an Asn1OutputStream.
         * <pre>
         * RecipientKeyIdentifier ::= Sequence {
         *     subjectKeyIdentifier SubjectKeyIdentifier,
         *     date GeneralizedTime OPTIONAL,
         *     other OtherKeyAttribute OPTIONAL
         * }
         *
         * SubjectKeyIdentifier ::= OCTET STRING
         * </pre>
         */
        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector(subjectKeyIdentifier);
			v.AddOptional(date, other);
			return new DerSequence(v);
        }
    }
}
#pragma warning restore
#endif
