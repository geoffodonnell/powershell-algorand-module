using System;
using System.ComponentModel;
using System.Management.Automation;

namespace Algorand.PowerShell.Model {

	[TypeConverter(typeof(NetworkModelPSTypeConverter))]
	public class NetworkModel {

		public string Name { get; set; }

		public string GenesisId { get; set; }

		public string GenesisHash { get; set; }

		public NetworkModel() { }

		public NetworkModel(NetworkConfiguration value) {

			Name = value.Name;
			GenesisId = value.GenesisId;
			GenesisHash = value.GenesisHash;	
		}

	}

	public class NetworkModelPSTypeConverter : PSTypeConverter {

		public override bool CanConvertFrom(object sourceValue, Type destinationType) {

			return CanConvertTo(sourceValue, destinationType); 
		}

		public override bool CanConvertTo(object sourceValue, Type destinationType) {

			if (sourceValue is NetworkModel) {
				return destinationType == typeof(string);
			}

			if (sourceValue is string) {
				return destinationType == typeof(NetworkModel);
			}

			return false;
		}

		public override object ConvertFrom(
			object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase) {

			return ConvertTo(sourceValue, destinationType, formatProvider, ignoreCase);
		}

		public override object ConvertTo(
			object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase) {

			// Not sure if this is neccessary
			if (sourceValue.GetType().IsAssignableFrom(destinationType)) {
				return sourceValue;
			}

			if (sourceValue is NetworkModel networkModel) {
				return networkModel.GenesisHash;
			}

			if (sourceValue is string asString) {
				return new NetworkModel(PsConfiguration.GetNetworkOrThrow(asString));
			}

			return null;
		}

	}

}
